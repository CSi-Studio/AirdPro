using System;
using System.Collections.Generic;
using System.IO;
using AirdSDK.Beans.Common;
using AirdSDK.Compressor;
using AirdSDK.Enums;
using AirdSDK.Exception;
using AirdSDK.Utils;

namespace AirdSDK.Parser;

public class ColumnParser
{
    /**
     * the aird file
     */
    public FileInfo airdFile;

    /**
     * the column index file. JSON format,with cjson
     */
    public string indexPath;

    /**
     * the airdInfo from the index file.
     */
    public Beans.ColumnInfo columnInfo;

    /**
     * mz precision
     */
    public double mzPrecision;

    /**
     * intensity precision
     */
    public double intPrecision;

    /**
     * File reader
     */
    public FileStream fs;

    public ColumnParser(string indexPath)
    {
        this.indexPath = indexPath;
        columnInfo = AirdScanUtil.LoadColumnInfo(indexPath);
        if (columnInfo == null) throw new ScanException(ResultCodeEnum.AIRD_INDEX_FILE_PARSE_ERROR);

        airdFile = new FileInfo(AirdScanUtil.GetAirdPathByColumnIndexPath(indexPath));
        fs = File.OpenRead(airdFile.FullName);

        mzPrecision = columnInfo.mzPrecision;
        intPrecision = columnInfo.intPrecision;
        ParseColumnIndex();
    }


    public void ParseColumnIndex()
    {
        List<AirdSDK.Beans.ColumnIndex> indexList = columnInfo.indexList;
        foreach (var columnIndex in indexList)
        {
            if (columnIndex.mzs == null)
            {
                byte[] mzsByte = ReadByte(columnIndex.startMzListPtr, columnIndex.endMzListPtr);
                int[] mzsAsInt = DecodeToSorted(mzsByte);
                columnIndex.mzs = mzsAsInt;
            }

            if (columnIndex.rts == null)
            {
                byte[] rtsByte = ReadByte(columnIndex.startRtListPtr, columnIndex.endRtListPtr);
                int[] rtsAsInt = DecodeToSorted(rtsByte);
                columnIndex.rts = rtsAsInt;
            }

            if (columnIndex.spectraIds == null)
            {
                byte[] spectraIdBytes = ReadByte(columnIndex.startSpectraIdListPtr, columnIndex.endSpectraIdListPtr);
                int[] spectraIds = Decode(spectraIdBytes);
                columnIndex.spectraIds = spectraIds;
            }

            if (columnIndex.intensities == null)
            {
                byte[] intensityBytes = ReadByte(columnIndex.startIntensityListPtr, columnIndex.endIntensityListPtr);
                int[] intensities = Decode(intensityBytes);
                columnIndex.intensities = intensities;
            }
        }
    }

    public byte[] ReadByte(long startPtr, long endPtr)
    {
        int delta = (int)(endPtr - startPtr);
        fs.Seek(startPtr, SeekOrigin.Begin);
        byte[] result = new byte[delta];
        fs.Read(result, 0, delta);
        return result;
    }

    public byte[] ReadByte(long startPtr, int delta)
    {
        fs.Seek(startPtr, SeekOrigin.Begin);
        byte[] result = new byte[delta];
        fs.Read(result, 0, delta);
        return result;
    }

    public Xic CalcXicByMz(double mz, double mzWindow)
    {
        return CalcXic(mz - mzWindow, mz + mzWindow, null, null, null);
    }

    public Xic CalcXicByWindow(double mz, double mzWindow, double? rt, double? rtWindow, double? precursorMz)
    {
        double? rtStart = null;
        double? rtEnd = null;
        if (rt != null && rtWindow != null)
        {
            rtStart = rt - rtWindow;
            rtEnd = rt + rtWindow;
        }
        return CalcXic(mz - mzWindow, mz + mzWindow, rtStart, rtEnd, precursorMz);
    }

    public Xic CalcXic(double mzStart, double mzEnd, double? rtStart, double? rtEnd, double? precursorMz)
    {
        if (columnInfo.indexList == null || columnInfo.indexList.Count == 0)
        {
            return null;
        }

        AirdSDK.Beans.ColumnIndex index = null;
        if (precursorMz != null)
        {
            foreach (var columnIndex in columnInfo.indexList)
            {
                if (columnIndex.range != null && columnIndex.range.start <= precursorMz &&
                    columnIndex.range.end > precursorMz)
                {
                    index = columnIndex;
                }
            }
        }
        else
        {
            index = columnInfo.indexList[0];
        }

        if (index == null)
        {
            return null;
        }

        int[] mzs = index.mzs;
        int start = (int)(mzStart * mzPrecision);
        int end = (int)(mzEnd * mzPrecision);
        IntPair leftMzPair = AirdMathUtil.BinarySearch(mzs, start);
        int leftMzIndex = leftMzPair.right;
        IntPair rightMzPair = AirdMathUtil.BinarySearch(mzs, end);
        int rightMzIndex = rightMzPair.left;

        int leftRtIndex = 0;
        int rightRtIndex = index.rts.Length - 1;
        if (rtStart != null)
        {
            IntPair leftRtPair = AirdMathUtil.BinarySearch(index.rts, (int)(rtStart * 1000));
            leftRtIndex = leftRtPair.right;
        }

        if (rtEnd != null)
        {
            IntPair rightRtPair = AirdMathUtil.BinarySearch(index.rts, (int)(rtEnd * 1000));
            rightRtIndex = rightRtPair.left;
        }

        int[] spectraIdLengths = index.spectraIds;
        int[] intensityLengths = index.intensities;
        int anchorIndex = leftMzIndex / 100000;
        long startPtr = index.anchors[anchorIndex];
        for (int i = anchorIndex * 100000; i < leftMzIndex; i++)
        {
            startPtr += spectraIdLengths[i];
            startPtr += intensityLengths[i];
        }

        List<Dictionary<int, double>> columnMapList = new List<Dictionary<int, double>>();
        Dictionary<int, double> map = new Dictionary<int, double>();
        for (int k = leftMzIndex; k <= rightMzIndex; k++)
        {
            byte[] spectraIdBytes = ReadByte(startPtr, spectraIdLengths[k]);
            startPtr += spectraIdLengths[k];
            byte[] intensityBytes = ReadByte(startPtr, intensityLengths[k]);
            startPtr += intensityLengths[k];
            int[] spectraIds = FastDecodeToSorted(spectraIdBytes);
            int[] ints = FastDecode(intensityBytes);
            //解码intensity
            for (int t = 0; t < spectraIds.Length; t++)
            {
                int spectraId = spectraIds[t];
                if (spectraId >= leftRtIndex && spectraId <= rightRtIndex)
                {
                    double intensity = ints[t];
                    if (intensity < 0)
                    {
                        intensity = Math.Pow(2, -intensity / 100000d);
                    }

                    if (map.ContainsKey(spectraId))
                    {
                        map[spectraId] = map[spectraId] + intensity / intPrecision;
                    }
                    else
                    {
                        map[spectraId] = intensity / intPrecision;
                    }
                    
                }
            }

            columnMapList.Add(map);
        }

        int rtRange = rightRtIndex - leftRtIndex + 1;
        double[] intensities = new double[rtRange];
        double[] rts = new double[rtRange];
        int iteration = 0;
        for (int i = leftRtIndex; i <= rightRtIndex; i++)
        {
            if (map.TryGetValue(i, out double value))
            {
                intensities[iteration] = value;
            }
            else
            {
                intensities[iteration] = 0d;
            }
            
            rts[iteration] = index.rts[i] / 1000d;
            iteration++;
        }
        
        return new Xic(rts, intensities);
    }

    public int[] DecodeToSorted(byte[] origin)
    {
        return new IntegratedVarByteWrapper().decode(ByteTrans.byteToInt(origin));
    }
    
    public int[] Decode(byte[] origin)
    {
        return new VarByteWrapper().decode(ByteTrans.byteToInt(origin));
    }
    
    public int[] FastDecodeToSorted(byte[] origin)
    {
        return new IntegratedVarByteWrapper().decode(ByteTrans.byteToInt(origin));
    }
    
    public int[] FastDecode(byte[] origin)
    {
        return new VarByteWrapper().decode(ByteTrans.byteToInt(origin));
    }
}
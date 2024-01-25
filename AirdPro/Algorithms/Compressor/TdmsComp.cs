using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AirdPro.Constants;
using AirdPro.Converters;
using AirdPro.Domains;
using AirdPro.Utils;
using AirdSDK.Beans;
using AirdSDK.Compressor;

namespace AirdPro.Algorithms.Compressor.Tdms;

public class TdmsComp
{
    public int MzPrecision = 100000;
    public bool IgnoreZero = true;
    public bool IsCentroid = false;
    public int IntensityPrecision = 10;
    
    public SortedIntComp MzIntComp;
    public ByteComp MzByteComp;
    public IntComp IntIntComp;
    public ByteComp IntByteComp;
    
    public TdmsComp(JobInfo jobInfo)
    {
       MzPrecision = jobInfo.config.mzPrecision;
       IgnoreZero = jobInfo.config.ignoreZeroIntensity;
       IsCentroid = jobInfo.config.centroid;
        
       MzIntComp = SortedIntComp.build(jobInfo.config.mzIntComp);
       MzByteComp = ByteComp.build(jobInfo.config.mzByteComp);

       IntIntComp = IntComp.build(jobInfo.config.intIntComp);
       IntByteComp = ByteComp.build(jobInfo.config.intByteComp);
    }

    public void CompressMs1(TdmsConverter converter, BlockIndex index)
    {
        Hashtable ms1Table = Hashtable.Synchronized(new Hashtable());
        int process = 0;
        for (var i = 0; i < converter.ms1List.Count; i++)
        {
            converter.JobInfo.log(null, Tag.progress(Tag.MS1, process, converter.ms1List.Count));
            MsIndex ms1Index = converter.ms1List[i];
            TempScan ts = new TempScan(ms1Index);
            TdmsSpectrum spectrum = converter.Spectra[i];
            Compress(spectrum, ts);
            ms1Table.Add(i, ts);
        }
        converter.WriteToFile(ms1Table, index);
    }

    public void Compress(TdmsSpectrum spectrum, TempScan ts)
    {
        List<double> mzData = new List<double>(spectrum.mzChannel.GetFirstData<double>());
        List<float> intData = new List<float>(spectrum.intChannel.GetFirstData<float>());
        var size = mzData.Count();
        if (size == 0)
        {
            ts.mzArrayBytes = new byte[0];
            ts.intArrayBytes = new byte[0];
            return;
        }
        
        int[] mzArray = new int[size];
        int[] intensityArray = new int[size];
        
        int j = 0;
        for (int t = 0; t < size; t++)
        {
            if (IgnoreZero && intData[t] == 0) continue;
            mzArray[j] = DataUtil.FetchMz(mzData[t], MzPrecision);
            // intensityArray[j] = Convert.ToInt32(Math.Log(intData[t]) / Math.Log(2) * 100);
            intensityArray[j] = DataUtil.FetchIntensity(intData[t], IntensityPrecision);
            j++;
        }
        int[] mzSubArray = new int[j];
        Array.Copy(mzArray, mzSubArray, j);
        int[] intensitySubArray = new int[j];
        Array.Copy(intensityArray, intensitySubArray, j);
        byte[] compressedMzArray = null;
        byte[] compressedIntArray = null;
        
        if (mzSubArray.Length == 0)
        {
            compressedMzArray = new byte[0];
        }
        else
        {
            compressedMzArray = ComboComp.encode(MzIntComp, MzByteComp, mzSubArray);
        }

        if (intensitySubArray.Length == 0)
        {
            compressedIntArray = new byte[0];
        }
        else
        {
            compressedIntArray = ComboComp.encode(IntIntComp, IntByteComp, intensitySubArray);
        }

        ts.tic = (long)intData.Sum();
        int index = DataUtil.FindMaxIndex(intData);
        ts.basePeakIntensity = intData[index];
        ts.basePeakMz = mzData[index];
        ts.mzArrayBytes = compressedMzArray;
        ts.intArrayBytes = compressedIntArray;
    }
}
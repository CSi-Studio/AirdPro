using System.Collections.Generic;
using System.IO;
using AirdSDK.Compressor;
using AirdSDK.Domains;
using AirdSDK.Enums;
using AirdSDK.Exception;

namespace AirdSDK.Parser;

public abstract class BaseParser
{
    public static int MAX_READ_SIZE = int.MaxValue / 100;

    /**
     * the aird file
     */
    public FileInfo airdFile;

    /**
     * the aird index file. JSON format
     */
    public FileInfo indexFile;

    /**
     * the airdInfo from the index file.
     */
    public AirdInfo airdInfo;

    /**
     * the m/z compressor
     */
    public Domains.Compressor mzCompressor;

    /**
     * the intensity compressor
     */
    public Domains.Compressor intCompressor;

    /**
     * 用于PASEF的压缩内核
     */
    public Domains.Compressor mobiCompressor;

    public double mzPrecision;
    public double intPrecision;
    public double mobiPrecision;

    /**
     * 使用的压缩内核
     */
    public SortedIntComp mzIntComp;

    public ByteComp mzByteComp;

    public IntComp intIntComp;
    public ByteComp intByteComp;

    public IntComp mobiIntComp;
    public ByteComp mobiByteComp;

    /**
     * Mobility 字典
     */
    public double[] mobiDict;

    public FileStream fs;

    public BaseParser()
    {
    }

    public BaseParser(string indexPath)
    {
        indexFile = new FileInfo(indexPath);
        airdInfo = AirdScanUtil.loadAirdInfo(indexFile);
        if (airdInfo == null) throw new ScanException(ResultCodeEnum.AIRD_INDEX_FILE_PARSE_ERROR);

        airdFile = new FileInfo(AirdScanUtil.getAirdPathByIndexPath(indexPath));
        var fs = File.OpenRead(airdFile.FullName);

        parseCompsFromAirdInfo();
        parserComboComp();
        parseMobilityDict();
    }

    public void parseCompsFromAirdInfo()
    {
        mzCompressor = fetchTargetCompressor(airdInfo.compressors, Domains.Compressor.TARGET_MZ);
        intCompressor = fetchTargetCompressor(airdInfo.compressors, Domains.Compressor.TARGET_INTENSITY);
        mobiCompressor = fetchTargetCompressor(airdInfo.compressors, Domains.Compressor.TARGET_MOBILITY);
        mzPrecision = mzCompressor.precision;
        intPrecision = intCompressor.precision;
        mobiPrecision = mobiCompressor.precision;
    }

    public void parserComboComp()
    {
        var mzMethods = mzCompressor.methods;
        if (mzMethods.Count == 2)
        {
            switch (mzMethods[0])
            {
                case "IBP":
                    mzIntComp = new IntegratedBinPackingWrapper(); //IBP
                    break;
                case "IVB":
                    mzIntComp = new IntegratedVarByteWrapper(); //IVB
                    break;
            }

            switch (mzMethods[1])
            {
                case "Zlib":
                    mzByteComp = new ZlibWrapper();
                    break;
                case "Brotli":
                    mzByteComp = new BrotliWrapper();
                    break;
                case "Snappy":
                    mzByteComp = new SnappyWrapper();
                    break;
                case "Zstd":
                    mzByteComp = new ZstdWrapper();
                    break;
            }
        }

        var intMethods = intCompressor.methods;
        if (intMethods.Count == 2)
        {
            switch (intMethods[0])
            {
                case "VB":
                    intIntComp = new VarByteWrapper();
                    break;
                case "BP":
                    intIntComp = new BinPackingWrapper();
                    break;
                case "Empty":
                    intIntComp = new Empty();
                    break;
            }

            switch (intMethods[1])
            {
                case "Zlib":
                    intByteComp = new ZlibWrapper();
                    break;
                case "Brotli":
                    intByteComp = new BrotliWrapper();
                    break;
                case "Snappy":
                    intByteComp = new SnappyWrapper();
                    break;
                case "Zstd":
                    intByteComp = new ZstdWrapper();
                    break;
            }
        }

        if (mobiCompressor != null)
        {
            var mobiMethods = mobiCompressor.methods;
            if (mobiMethods.Count == 2)
            {
                switch (mobiMethods[0])
                {
                    case "VB":
                        mobiIntComp = new VarByteWrapper();
                        break;
                    case "BP":
                        mobiIntComp = new BinPackingWrapper();
                        break;
                    case "Empty":
                        mobiIntComp = new Empty();
                        break;
                }

                switch (mobiMethods[1])
                {
                    case "Zlib":
                        mobiByteComp = new ZlibWrapper();
                        break;
                    case "Brotli":
                        mobiByteComp = new BrotliWrapper();
                        break;
                    case "Snappy":
                        mobiByteComp = new SnappyWrapper();
                        break;
                    case "Zstd":
                        mobiByteComp = new ZstdWrapper();
                        break;
                }
            }
        }
    }

    /**
   * 必须读取索引文件以及Aird二进制文件才可以获取Dict字典
   */
    public void parseMobilityDict()
    {
        MobiInfo mobiInfo = airdInfo.mobiInfo;
    if ("TIMS".Equals(mobiInfo.type)) {
        fs.Seek(mobiInfo.dictStart, SeekOrigin.Begin);
        int delta = (int)(mobiInfo.dictEnd - mobiInfo.dictStart);
        byte[] result = new byte[delta];
        fs.Read(result,0,delta);
        int[] mobiArray = new IntegratedVarByteWrapper().decode(ByteTrans.byteToInt(new ZstdWrapper().decode(result)));
        double[] mobiDArray = new double[mobiArray.Length];
        for (int i = 0; i<mobiArray.Length; i++) {
            mobiDArray[i] = mobiArray[i] / mobiPrecision;
        }
        this.mobiDict = mobiDArray;
    }
}

    public static Domains.Compressor fetchTargetCompressor(List<Domains.Compressor> compressors, string target)
    {
        if (compressors == null) return null;
        foreach (var compressor in compressors)
            if (compressor.target.Equals(target))
                return compressor;
        return null;
    }
}
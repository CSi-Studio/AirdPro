using System;
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
    public FileStream airdFile;

    /**
     * the aird index file. JSON format
     */
    public FileStream indexFile;

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

    public BaseParser()
    {
    }

    public BaseParser(string indexPath)
    {
        this.indexFile = new FileStream(indexPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        airdInfo = AirdScanUtil.loadAirdInfo(indexFile);
        if (airdInfo == null)
        {
            throw new ScanException(ResultCodeEnum.AIRD_INDEX_FILE_PARSE_ERROR);
        }

        this.airdFile = new FileStream(AirdScanUtil.getAirdPathByIndexPath(indexPath), FileMode.Open, FileAccess.Read,
            FileShare.Read);
        try
        {
            raf = new RandomAccessFile(airdFile, "r");
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine(e.StackTrace);
            throw new ScanException(ResultCodeEnum.AIRD_FILE_PARSE_ERROR);
        }

        parseCompsFromAirdInfo();
        parserComboComp();
        parseMobilityDict();
    }
}
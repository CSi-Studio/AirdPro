using AirdSDK.Beans;
using AirdSDK.Enums;

namespace AirdSDK.Parser;

public class DIAParser : BaseParser
{
    public DIAParser(string indexFilePath) : base(indexFilePath)
    {
    }

    public DIAParser(string indexFilePath, AirdInfo airdInfo) : base(indexFilePath, airdInfo)
    {
    }

    /**
     * 构造函数
     *
     * @param airdPath      aird file path
     * @param mzCompressor  mz compressor
     * @param intCompressor intensity compressor
     * @throws ScanException scan exception
     */
    public DIAParser(string airdPath, Beans.Compressor mzCompressor, Beans.Compressor intCompressor) : base(airdPath,
        mzCompressor, intCompressor, null, AirdType.DIA)
    {
    }
}
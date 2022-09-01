using AirdSDK.Beans;

namespace AirdSDK.Parser;

public class CommonParser : BaseParser
{
    public CommonParser(string indexFilePath) : base(indexFilePath)
    {
    }

    public CommonParser(string indexFilePath, AirdInfo airdInfo) : base(indexFilePath, airdInfo)
    {
    }
}
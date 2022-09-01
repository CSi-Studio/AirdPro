using AirdSDK.Beans;

namespace AirdSDK.Parser;

public class PRMParser : BaseParser
{
    public PRMParser(string indexFilePath) : base(indexFilePath)
    {
    }

    public PRMParser(string indexFilePath, AirdInfo airdInfo) : base(indexFilePath, airdInfo)
    {
    }
}
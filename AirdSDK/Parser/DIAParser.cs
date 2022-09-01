using AirdSDK.Beans;

namespace AirdSDK.Parser;

public class DIAParser : BaseParser
{
    public DIAParser(string indexFilePath) : base(indexFilePath)
    {
    }

    public DIAParser(string indexFilePath, AirdInfo airdInfo) : base(indexFilePath, airdInfo)
    {
    }
}
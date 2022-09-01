using AirdSDK.Beans;

namespace AirdSDK.Parser;

public class DIAPasefParser : BaseParser
{
    public DIAPasefParser(string indexFilePath) : base(indexFilePath)
    {
    }

    public DIAPasefParser(string indexFilePath, AirdInfo airdInfo) : base(indexFilePath, airdInfo)
    {
    }
}
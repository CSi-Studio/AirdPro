using AirdSDK.Beans;

namespace AirdSDK.Parser;

public class DDAPasefParser : BaseParser
{
    public DDAPasefParser(string indexFilePath) : base(indexFilePath)
    {
    }

    public DDAPasefParser(string indexFilePath, AirdInfo airdInfo) : base(indexFilePath, airdInfo)
    {
    }
}
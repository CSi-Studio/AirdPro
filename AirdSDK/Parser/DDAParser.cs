using AirdSDK.Beans;

namespace AirdSDK.Parser;

public class DDAParser : BaseParser
{
    public DDAParser(string indexFilePath) : base(indexFilePath)
    {
    }

    public DDAParser(string indexFilePath, AirdInfo airdInfo) : base(indexFilePath, airdInfo)
    {
    }
}
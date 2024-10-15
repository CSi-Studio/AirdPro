using System.IO;

namespace AirdPro.csimzMLParser.obo
{
    public interface IOBOLoader
    {
        FileStream GetInputStream(string location);
    }
}

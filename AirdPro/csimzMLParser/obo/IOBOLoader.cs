using System.IO;

namespace AirdPro.csimzMLParser.obo
{
    public interface IOBOLoader
    {
        Stream GetInputStream(string location);
    }
}

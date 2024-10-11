using System.IO;

namespace AirdPro.csimzMLParser.data
{
    public class BinaryDataStorage : DataStorage
    {
        public BinaryDataStorage(FileInfo dataFile, bool openForWriting) : base(dataFile, openForWriting)
        {
            
        }        
    }
}

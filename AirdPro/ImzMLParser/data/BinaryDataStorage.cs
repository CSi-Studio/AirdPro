using System.IO;

namespace AirdPro.ImzMLParser.data
{
    public class BinaryDataStorage : DataStorage
    {
        public BinaryDataStorage(FileInfo dataFile, bool openForWriting) : base(dataFile, openForWriting)
        {
            
        }        
    }
}

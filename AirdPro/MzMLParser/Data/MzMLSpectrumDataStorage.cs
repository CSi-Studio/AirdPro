using System.IO;

namespace AirdPro.csimzMLParser.data
{
    public class MzMLSpectrumDataStorage : DataStorage
    {
        protected Base64DataStorage base64DataStorage;

        public MzMLSpectrumDataStorage(FileInfo dataFile) : base(dataFile)
        {
	        base64DataStorage = new Base64DataStorage(dataFile);
        }
        public Base64DataStorage GetBase64DataStorage()
        {
            return base64DataStorage;
        }

        public override void Close()
        {
            base.Close();
            base64DataStorage.Close();
        }
    }
}

using System;
using System.IO;
using System.Text;

namespace AirdPro.csimzMLParser.data
{
    public class Base64DataStorage : DataStorage
    {
        public Base64DataStorage(FileInfo dataFile) : base(dataFile)
        {
           
        }
    
        public override byte[] GetData(long offset, int length) 
        {
            byte[] buffer = base.GetData(offset, length);
            return Convert.FromBase64String(Encoding.UTF8.GetString(buffer));
        }

    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.ImzMLParser.data
{
    public class Base64DataStorage : DataStorage
    {
        public Base64DataStorage(FileInfo dataFile) : base(dataFile)
        {
           
        }
    
        public override byte[] GetData(long offset, int length) 
        {
            byte[] buffer = base.GetData(offset, length);
            string base64Encoded = Convert.ToBase64String(buffer);
            byte[] decodedBytes = Convert.FromBase64String(base64Encoded);
            return decodedBytes;
        }

    }
}

using System;
using System.IO;

namespace AirdPro.ImzMLParser.data
{
    public class ZstdDataTransform : IDataTransform
    {
        protected int arrayLengthInBytes;
        protected int compressionLevel;

        public ZstdDataTransform(int arrayLengthInBytes) : this(arrayLengthInBytes, 3)
        {
            
        }

        public ZstdDataTransform(int arrayLengthInBytes, int compressionLevel)
        {
            this.arrayLengthInBytes = arrayLengthInBytes;
            this.compressionLevel = compressionLevel;
        }

        public byte[] ForwardTransform(byte[] data)
        {
            return data;
        }

        public byte[] ReverseTransform(byte[] data)
        {
            return data;
            
        }


    }
}

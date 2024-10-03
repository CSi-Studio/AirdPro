using Ionic.Zlib;

namespace AirdPro.csimzMLParser.data
{
    public class ZlibDataTransform : IDataTransform
    {
        public byte[] ForwardTransform(byte[] data)
        {
            return ZlibStream.CompressBuffer(data);            
        }

        public byte[] ReverseTransform(byte[] data)
        {
            return ZlibStream.UncompressBuffer(data);
        }
    }
}

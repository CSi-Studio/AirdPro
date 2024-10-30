using AirdSDK.Compressor;

namespace AirdPro.csimzMLParser.data
{
    public class ZstdDataTransform : IDataTransform
    {
        public byte[] ForwardTransform(byte[] data)
        {
            return new ZstdWrapper().encode(data);           
        }

        public byte[] ReverseTransform(byte[] data)
        {
            return new ZstdWrapper().decode(data);
        }
    }
}

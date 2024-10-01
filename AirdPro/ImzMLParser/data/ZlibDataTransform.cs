using System.IO;
using System.IO.Compression;

namespace AirdPro.ImzMLParser.data
{
    public class ZlibDataTransform : IDataTransform
    {
        protected static readonly int BYTE_BUFFER_SIZE = 1 << 20; // 更清晰的位运算表示

        public virtual byte[] ForwardTransform(byte[] data)
        {
            var outputStream = new MemoryStream();
            using (var compressor = new DeflateStream(outputStream, CompressionMode.Compress))
            {
                compressor.Write(data, 0, data.Length);
                compressor.Flush(); 
            }
            return outputStream.ToArray();
        }

        public virtual byte[] ReverseTransform(byte[] data)
        {
            var outputStream = new MemoryStream();
            using (var inputStream = new MemoryStream(data))
            using (var decompressor = new DeflateStream(inputStream, CompressionMode.Decompress))
            {
                decompressor.CopyTo(outputStream);
            }
            return outputStream.ToArray();
        }
    }
}

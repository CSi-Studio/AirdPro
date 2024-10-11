using AirdSDK.Compressor;

namespace AirdPro.csimzMLParser.data
{
    public class ZstdDataTransform : IDataTransform
    {
        //protected static readonly int BYTE_BUFFER_SIZE = 2 ^ 20;
        public byte[] ForwardTransform(byte[] data)
        {
            return new ZstdWrapper().encode(data);
            /*using (var compressedStream = new MemoryStream())
            {
                using (var compressionStream = new CompressionStream(compressedStream))
                {
                    compressionStream.Write(data, 0, data.Length);
                }
                compressedStream.Position = 0;
                return compressedStream.ToArray();
            }*/
        }

        public byte[] ReverseTransform(byte[] data)
        {
            return new ZstdWrapper().decode(data);
            /*using (var compressedStream = new MemoryStream(data))
            {
                using (var decompressionStream = new DecompressionStream(compressedStream))
                {
                    var decompressedData = new byte[BYTE_BUFFER_SIZE]; 
                    decompressionStream.Read(decompressedData, 0, decompressedData.Length);
                    return decompressedData;
                }
            }*/
        }
    }
}

using K4os.Compression.LZ4;
using System;
using System.Linq;

namespace AirdPro.csimzMLParser.data
{
    public class LZ4DataTransform : IDataTransform
    {
        protected static readonly int BYTE_BUFFER_SIZE = 2 ^ 20;
        public byte[] ForwardTransform(byte[] data)
        {
            int compressedLength = LZ4Codec.MaximumOutputSize(data.Length);
            byte[] compressedData = new byte[compressedLength];

            int bytesWritten = LZ4Codec.Encode(data, 0, data.Length, compressedData, 0, compressedData.Length);
            return compressedData.Take(bytesWritten).ToArray();

        }

        public byte[] ReverseTransform(byte[] data)
        {
            byte[] decompressedData = new byte[BYTE_BUFFER_SIZE];
            int bytesRead = LZ4Codec.Decode(data, 0, data.Length, decompressedData, 0, decompressedData.Length);
            if (bytesRead != decompressedData.Length)
            {
                throw new InvalidOperationException("Decompressed data length does not match expected length.");
            }
            return decompressedData;
        }
    }
}

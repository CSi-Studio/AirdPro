using System.IO;
using AirdPro.Algorithms;
using ZstdNet;

namespace Compress
{
    public class ZSTD:ByteComp
    {

        //使用zstd将byte数组压缩
        public byte[] encode(byte[] data)
        {
            var compressor = new Compressor();
            var compressedData = compressor.Wrap(data);
            return compressedData;
        }

        public byte[] decode(byte[] data)
        {
            var decompressor = new Decompressor();
            var decompressedData = decompressor.Unwrap(data);
            return decompressedData;
        }

    }
}
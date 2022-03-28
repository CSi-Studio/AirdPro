using System.IO;
using AirdPro.Algorithms;
using AirdPro.Constants;
using Compress.Enums;
using ZstdNet;

namespace Compress
{
    public class ZstdWrapper:ByteComp
    {

        //使用zstd将byte数组压缩
        public override string getName()
        {
            return ByteCompType.Zstd.ToString();
        }

        public override byte[] encode(byte[] data)
        {
            var compressor = new Compressor();
            var compressedData = compressor.Wrap(data);
            return compressedData;
        }

        public override byte[] decode(byte[] data)
        {
            var decompressor = new Decompressor();
            var decompressedData = decompressor.Unwrap(data);
            return decompressedData;
        }

    }
}
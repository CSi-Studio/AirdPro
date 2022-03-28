using System.IO;
using AirdPro.Algorithms;
using AirdPro.Constants;
using Compress.Enums;
using IronSnappy;
using ZstdNet;

namespace Compress
{
    public class SnappyWrapper:ByteComp
    {

        //使用zstd将byte数组压缩
        public override string getName()
        {
            return ByteCompType.Snappy.ToString();
        }

        public override byte[] encode(byte[] data)
        {
            byte[] compressed = Snappy.Encode(data);
            return compressed;
        }

        public override byte[] decode(byte[] data)
        {
            byte[] uncompressed = Snappy.Decode(data);
            return uncompressed;
        }

    }
}
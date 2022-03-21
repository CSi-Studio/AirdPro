using System.IO;
using AirdPro.Algorithms;
using AirdPro.Constants;
using IronSnappy;
using ZstdNet;

namespace Compress
{
    public class SnappyWrapper:ByteComp
    {

        //使用zstd将byte数组压缩
        public string getName()
        {
            return ByteCompType.Snappy.ToString();
        }

        public byte[] encode(byte[] data)
        {
            byte[] compressed = Snappy.Encode(data);
            return compressed;
        }

        public byte[] decode(byte[] data)
        {
            byte[] uncompressed = Snappy.Decode(data);
            return uncompressed;
        }

    }
}
using System.IO;
using AirdPro.Algorithms;
using IronSnappy;
using ZstdNet;

namespace Compress
{
    public class Snappier:ByteComp
    {

        //使用zstd将byte数组压缩
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
using System.IO;
using AirdPro.Constants;
using Compress;
using Compress.Enums;
using Ionic.Zlib;

namespace AirdPro.Algorithms
{
    public class ZlibWrapper:ByteComp
    {
        //使用zlib将byte数组压缩
        public override string getName()
        {
            return ByteCompType.Zlib.ToString();
        }

        public override byte[] encode(byte[] data)
        {
            using (var ms = new MemoryStream())
            {
                Stream compressor = new ZlibStream(ms, CompressionMode.Compress, CompressionLevel.Default);

                ZlibBaseStream.CompressBuffer(data, compressor);
                return ms.ToArray();
            }
        }

        //使用zlib将byte数组解压缩
        public override byte[] decode(byte[] data)
        {
            return ZlibStream.UncompressBuffer(data);
        }
    }
}
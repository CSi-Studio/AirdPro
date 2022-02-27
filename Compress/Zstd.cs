using System.IO;
using AirdPro.Algorithms;
using ZstdNet;

namespace Compress
{
    public class Zstd:ByteComp
    {

        //使用zstd将byte数组压缩
        public byte[] encode(byte[] data)
        {
            using (var ms = new MemoryStream())
            {
                CompressionStream compressionStream = new CompressionStream(ms);
                compressionStream.Write(data, 0, data.Length);
                compressionStream.FlushAsync().GetAwaiter().GetResult();
                byte[] result = ms.ToArray();
                return result;
            }
        }

        //使用zstd将float数组压缩为byte数组
        public byte[] encode(float[] target)
        {
            var targetArray = ByteTrans.floatToByte(target);
            var compressedArray = encode(targetArray);
            return compressedArray;
        }

        public byte[] decode(byte[] data)
        {
            using (var result = new MemoryStream())
            {
                DecompressionStream decompressionStream = new DecompressionStream(new MemoryStream(data));
                decompressionStream.CopyTo(result);
                return result.ToArray();
            }
        }

        public float[] decodeToFloat(byte[] data)
        {
            using (var result = new MemoryStream())
            {
                DecompressionStream decompressionStream = new DecompressionStream(new MemoryStream(data));
                decompressionStream.CopyTo(result);
                byte[] origin = result.ToArray();
                return ByteTrans.byteToFloat(origin);
            }
        }

    }
}
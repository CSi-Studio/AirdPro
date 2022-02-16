using System.IO;
using Ionic.Zlib;

namespace AirdPro.Algorithms
{
    public class Zlib
    {
        //使用zlib将byte数组压缩
        public static byte[] encode(byte[] data)
        {
            using (var ms = new MemoryStream())
            {
                Stream compressor = new ZlibStream(ms, CompressionMode.Compress, CompressionLevel.Default);

                ZlibBaseStream.CompressBuffer(data, compressor);
                return ms.ToArray();
            }
        }

        //使用zlib将int数组压缩为byte数组
        public static byte[] encode(int[] target)
        {
            var targetArray = ByteTrans.intToByte(target);
            var compressedArray = encode(targetArray);
            return compressedArray;
        }

        //使用zlib将float数组压缩为byte数组
        public static byte[] encode(float[] target)
        {
            var targetArray = ByteTrans.floatToByte(target);
            var compressedArray = encode(targetArray);
            return compressedArray;
        }

        //使用zlib将byte数组解压缩
        public static byte[] decode(byte[] data)
        {
            return ZlibStream.UncompressBuffer(data);
        }

        //使用zlib将byte数组解压缩为float数组
        public static float[] decodeToFloat(byte[] compressed)
        {
            byte[] array = ZlibStream.UncompressBuffer(compressed);
            return ByteTrans.byteToFloat(array);
        }

        //使用zlib将byte数组解压缩为int数组
        public static int[] decodeToInt(byte[] compressed)
        {
            byte[] array = ZlibStream.UncompressBuffer(compressed);
            return ByteTrans.byteToInt(array);
        }
    }
}
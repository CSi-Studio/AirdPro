﻿using System.IO;
using Compress;
using Ionic.Zlib;

namespace AirdPro.Algorithms
{
    public class Zlib:ByteComp
    {
        //使用zlib将byte数组压缩
        public byte[] encode(byte[] data)
        {
            using (var ms = new MemoryStream())
            {
                Stream compressor = new ZlibStream(ms, CompressionMode.Compress, CompressionLevel.Default);

                ZlibBaseStream.CompressBuffer(data, compressor);
                return ms.ToArray();
            }
        }

        //使用zlib将byte数组解压缩
        public byte[] decode(byte[] data)
        {
            return ZlibStream.UncompressBuffer(data);
        }
    }
}
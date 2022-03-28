using System.IO;
using AirdPro.Algorithms;
using AirdPro.Constants;
using BrotliSharpLib;
using Compress.Enums;
using IronSnappy;
using ZstdNet;

namespace Compress
{
    public class BrotliWrapper:ByteComp
    {
        public override string getName()
        {
            return ByteCompType.Brotli.ToString();
        }

        public override byte[] encode(byte[] data)
        {
            byte[] compressed = Brotli.CompressBuffer(data, 0, data.Length);
            return compressed;
        }

        public override byte[] decode(byte[] data)
        {
            byte[] uncompressed = Brotli.DecompressBuffer(data, 0, data.Length);
            return uncompressed;
        }

    }
}
using System.IO;
using AirdPro.Algorithms;
using BrotliSharpLib;
using IronSnappy;
using ZstdNet;

namespace Compress
{
    public class Brotlier:ByteComp
    {
        public byte[] encode(byte[] data)
        {
            byte[] compressed = Brotli.CompressBuffer(data, 0, data.Length);
            return compressed;
        }

        public byte[] decode(byte[] data)
        {
            byte[] uncompressed = Brotli.DecompressBuffer(data, 0, data.Length);
            return uncompressed;
        }

    }
}
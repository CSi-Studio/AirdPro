using System;
using AirdPro.Algorithms;
using AirdPro.Constants;
using Compress.Enums;

namespace Compress;

public abstract class ByteComp
{
    public abstract string getName();
    public abstract byte[] encode(byte[] uncompressed);
    public abstract byte[] decode(byte[] compressed);

    public static ByteComp build(ByteCompType type)
    {
        switch (type)
        {
            case ByteCompType.Brotli:
                return new BrotliWrapper();
            case ByteCompType.Snappy:
                return new SnappyWrapper();
            case ByteCompType.Zstd:
                return new ZstdWrapper();
            case ByteCompType.Zlib:
                return new ZlibWrapper();
            default: throw new Exception("No Implementation for " + type);
        }
    }
}
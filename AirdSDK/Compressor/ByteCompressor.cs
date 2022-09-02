using AirdSDK.Enums;

namespace AirdSDK.Compressor;

public class ByteCompressor
{
    private ByteCompType byteCompType;

    public ByteCompressor(ByteCompType type)
    {
        this.byteCompType = type;
    }

    public byte[] encode(byte[] bytes)
    {
        switch (byteCompType)
        {
            case ByteCompType.Zlib: return new ZlibWrapper().encode(bytes);
            case ByteCompType.Snappy: return new SnappyWrapper().encode(bytes);
            case ByteCompType.Brotli: return new BrotliWrapper().encode(bytes);
            case ByteCompType.Zstd: return new ZstdWrapper().encode(bytes);
            default: return null;
        }
    }

    public byte[] decode(byte[] bytes)
    {
        return decode(bytes, 0, bytes.Length);
    }

    /**
     * decompress the data with zlib at a specified start and length
     *
     * @param bytes  data to be decoded
     * @param start  the start position of the data array
     * @param length the length for compressor to decode
     * @return decompressed data
     */
    public byte[] decode(byte[] bytes, int start, int length)
    {
        switch (byteCompType)
        {
            case ByteCompType.Zlib: return new ZlibWrapper().decode(bytes, start, length);
            case ByteCompType.Snappy: return new SnappyWrapper().decode(bytes, start, length);
            case ByteCompType.Brotli: return new BrotliWrapper().decode(bytes, start, length);
            case ByteCompType.Zstd: return new ZstdWrapper().decode(bytes, start, length);
            default: return null;
        }
    }
}
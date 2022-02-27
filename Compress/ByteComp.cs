namespace Compress;

public interface ByteComp
{
    public byte[] encode(byte[] uncompressed);
    public byte[] decode(byte[] compressed);
}
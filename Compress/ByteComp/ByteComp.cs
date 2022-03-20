namespace Compress;

public interface ByteComp
{
    public string getName();
    public byte[] encode(byte[] uncompressed);
    public byte[] decode(byte[] compressed);
}
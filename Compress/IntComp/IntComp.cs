namespace Compress;

public interface IntComp
{
    public int[] encode(int[] uncompressed);
    public int[] decode(int[] compressed);
}
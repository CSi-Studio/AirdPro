namespace Compress;

public interface IntComp
{
    public string getName();
    public int[] encode(int[] uncompressed);
    public int[] decode(int[] compressed);
}
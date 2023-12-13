using AirdSDK.Compressor;

namespace AirdPro.Algorithms;

public class SortIntEmpty: SortedIntComp
{
    public override string getName()
    {
        return "Empty";
    }

    public override int[] encode(int[] uncompressed)
    {
        return uncompressed;
    }

    public override int[] decode(int[] compressed)
    {
        return compressed;
    }
}
using AirdPro.Constants;
using CSharpFastPFOR;

namespace Compress
{
    public class Simple16Wrapper:IntComp
    {
        public string getName()
        {
            return IntCompType.Simple.ToString();
        }

        public int[] encode(int[] uncompressed)
        {
            int[] compressedInts = new IntCompressor(new Simple16()).compress(uncompressed);
            return compressedInts;
        }
        
        public int[] decode(int[] compressed)
        {
            int[] sortedInts = new IntCompressor(new Simple16()).uncompress(compressed);
            return sortedInts;
        }
    }
}
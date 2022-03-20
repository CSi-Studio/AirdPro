using AirdPro.Constants;
using CSharpFastPFOR;

namespace Compress
{
    public class OptPFDS16Ser:IntComp
    {
        public string getName()
        {
            return IntCompType.OptPFD.ToString();
        }

        public int[] encode(int[] uncompressed)
        {
            int[] compressedInts = new IntCompressor(new OptPFDS16()).compress(uncompressed);
            return compressedInts;
        }

        public int[] decode(int[] compressed)
        {
            int[] sortedInts = new IntCompressor(new OptPFDS16()).uncompress(compressed);
            return sortedInts;
        }
    }
}
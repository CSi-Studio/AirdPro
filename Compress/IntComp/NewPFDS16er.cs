using AirdPro.Constants;
using CSharpFastPFOR;

namespace Compress
{
    public class NewPFDS16er : IntComp
    {
        public string getName()
        {
            return IntCompType.NewPFD.ToString();
        }

        public int[] encode(int[] uncompressed)
        {
            int[] compressedInts = new IntCompressor(new NewPFDS16()).compress(uncompressed);
            return compressedInts;
        }

        public int[] decode(int[] compressed)
        {
            int[] sortedInts = new IntCompressor(new NewPFDS16()).uncompress(compressed);
            return sortedInts;
        }
    }
}
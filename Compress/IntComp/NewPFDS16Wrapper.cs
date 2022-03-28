using AirdPro.Constants;
using CSharpFastPFOR;

namespace Compress
{
    public class NewPFDS16Wrapper : IntComp
    {
        public override string getName()
        {
            return IntCompType.NewPFD.ToString();
        }

        public override int[] encode(int[] uncompressed)
        {
            int[] compressedInts = new IntCompressor(new NewPFDS16()).compress(uncompressed);
            return compressedInts;
        }

        public override int[] decode(int[] compressed)
        {
            int[] sortedInts = new IntCompressor(new NewPFDS16()).uncompress(compressed);
            return sortedInts;
        }
    }
}
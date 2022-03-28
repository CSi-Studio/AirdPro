using AirdPro.Constants;
using CSharpFastPFOR;

namespace Compress
{
    public class Empty : IntComp
    {
        public override string getName()
        {
            return IntCompType.Empty.ToString();
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
}
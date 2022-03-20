using AirdPro.Constants;
using CSharpFastPFOR;

namespace Compress
{
    public class Empty:IntComp
    {
        public string getName()
        {
            return IntCompType.Empty.ToString();
        }

        public int[] encode(int[] uncompressed)
        {
            return uncompressed;
        }
        
        public int[] decode(int[] compressed)
        {
            return compressed;
        }
    }
}
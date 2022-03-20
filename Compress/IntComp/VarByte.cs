using AirdPro.Constants;
using CSharpFastPFOR;

namespace Compress
{
    public class VarByte:IntComp
    {
        //使用VariableByte算法将未排序的int数组进行压缩
        public string getName()
        {
            return IntCompType.VB.ToString();
        }

        public int[] encode(int[] uncompressed)
        {
            int[] compressedInts = new IntCompressor(new VariableByte()).compress(uncompressed);
            return compressedInts;
        }


        //使用VariableByte算法对已经压缩的int数组进行解压缩
        public int[] decode(int[] compressed)
        {
            int[] sortedInts = new IntCompressor(new VariableByte()).uncompress(compressed);
            return sortedInts;
        }
    }
}
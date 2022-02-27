using CSharpFastPFOR;
using CSharpFastPFOR.Differential;

namespace Compress
{
    public class IntegratedVarByte:IntComp
    {
        //使用VariableByte算法将排序了的int数组进行压缩
        public int[] encode(int[] uncompressed)
        {
            int[] compressedInts = new IntegratedIntCompressor(new IntegratedVariableByte()).compress(uncompressed);
            return compressedInts;
        }

        //使用VariableByte算法对已经压缩的int数组进行解压缩
        public int[] decode(int[] compressed)
        {
            int[] sortedInts = new IntegratedIntCompressor(new IntegratedVariableByte()).compress(compressed);
            return sortedInts;
        }
    }
}
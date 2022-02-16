using CSharpFastPFOR;
using CSharpFastPFOR.Differential;

namespace Compress
{
    public class VarByte
    {
        //使用VariableByte算法将排序了的int数组进行压缩
        public static int[] encode(int[] uncompressed, bool sorted)
        {
            int[] compressedInts = sorted ? new IntegratedIntCompressor(new IntegratedVariableByte()).compress(uncompressed) :
                new IntCompressor(new VariableByte()).compress(uncompressed);
            return compressedInts;
        }


        //使用VariableByte算法对已经压缩的int数组进行解压缩
        public static int[] decode(int[] compressed, bool sorted)
        {
            int[] sortedInts = sorted ? new IntegratedIntCompressor(new IntegratedVariableByte()).compress(compressed) :
                new IntCompressor(new VariableByte()).compress(compressed);
            return sortedInts;
        }
    }
}
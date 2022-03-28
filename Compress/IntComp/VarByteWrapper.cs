using Compress.Enums;
using CSharpFastPFOR;

namespace Compress
{
    public class VarByteWrapper:IntComp
    {
        //使用VariableByte算法将未排序的int数组进行压缩
        public override string getName()
        {
            return IntCompType.VB.ToString();
        }

        public override int[] encode(int[] uncompressed)
        {
            int[] intValues = new IntCompressor(new VariableByte()).compress(uncompressed);
            return intValues;
        }

        //使用VariableByte算法对已经压缩的int数组进行解压缩
        public override int[] decode(int[] compressed)
        {
            int[] intValues = new IntCompressor(new VariableByte()).uncompress(compressed);
            return intValues;
        }
    }
}
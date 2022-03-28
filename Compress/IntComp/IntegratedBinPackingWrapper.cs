using AirdPro.Constants;
using CSharpFastPFOR.Differential;

namespace Compress
{
    public class IntegratedBinPackingWrapper:SortedIntComp
    {
        //使用FastPfor算法将排序了的int数组进行压缩,注意:target数组必须是排序后的数组
        public override string getName()
        {
            return SortedIntCompType.IBP.ToString();
        }

        public override int[] encode(int[] uncompressed)
        {
            int[] compressedInts = new IntegratedIntCompressor().compress(uncompressed);
            return compressedInts;
        }

        //使用PFor算法对已经压缩的int数组进行解压缩
        public override int[] decode(int[] compressed)
        {
            int[] sortedInts = new IntegratedIntCompressor().uncompress(compressed);
            return sortedInts;
        }

    }
}
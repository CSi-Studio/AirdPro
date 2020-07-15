using System.Collections.Generic;

namespace AirdPro.Domains.Aird
{
    public class SwathIndex
    {
        //1: ms1 swath block, 2: ms2 swath block
        public int level;
        //在文件中的开始位置
        public long startPtr;
        //在文件中的结束位置
        public long endPtr;
        //SWATH块对应的WindowRange
        public WindowRange range;
        //当msLevel=1时,本字段为Swath Block中每一个MS1谱图的序号,,当msLevel=2时本字段为Swath Block中每一个MS2谱图对应的MS1谱图的序列号,MS2谱图本身不需要记录序列号
        public List<int> nums = new List<int>();
        //一个Swath块中所有子谱图的rt时间列表
        public List<float> rts = new List<float>();
        //一个Swath块中所有子谱图的mz的压缩后的大小列表
        public List<long> mzs = new List<long>();
        //一个Swath块中所有子谱图的intenisty的压缩后的大小列表
        public List<long> ints= new List<long>();
        //用于存储KV键值对
        public string features;
    }
}

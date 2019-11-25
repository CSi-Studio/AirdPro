using System;
using System.Collections.Generic;
using Propro.Structs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propro_Client.Domains.Aird
{
    public class BlockIndex
    {
        //1:MS1;2:MS2
        public int level;
        //在文件中的开始位置
        public long startPtr;
        //在文件中的结束位置
        public long endPtr;
        //每一个MS2对应的MS1序号;MS1中为空
        public int num;
        //每一个MS2对应的前体窗口;MS1中为空
        public List<WindowRange> rangeList;
        //当msLevel=1时,本字段为每一个MS1谱图的序号,,当msLevel=2时本字段为每一个MS2谱图序列号
        public List<int> nums = new List<int>();
        //所有该块中的rt时间列表
        public List<float> rts = new List<float>();
        //一个块中所有子谱图的mz的压缩后的大小列表
        public List<long> mzs = new List<long>();
        //一个块中所有子谱图的intenisty的压缩后的大小列表
        public List<long> ints = new List<long>();
        //用于存储KV键值对
        public string features;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propro.Domains
{
    class ScanIndex
    {
        //0 代表Swath Block Index,包含了一个完整的Swath窗口中的所有谱图,msLevel
        public int level;
        //retention time
        public float rt;
        //key为对应的文件类型, @see PositionType.class,positionMap
        public Hashtable pos;
        //序号
        public int num;
        //如果是ms2,本字段是对应的ms1的num
        public int pNum;
        //前体的荷质比,precursor mz
        public float mz;
        //前体的荷质比窗口开始位置,已经经过ExperimentDO.overlap参数调整,precursor mz
        public float mzStart;
        //前体的荷质比窗口结束位置,已经经过ExperimentDO.overlap参数调整,precursor mz
        public float mzEnd;
        //原始文件中前体的荷质比窗口开始位置,未经过ExperimentDO.overlap参数调整,precursor mz
        public float oMzStart;
        //原始文件中前体的荷质比窗口结束位置,未经过ExperimentDO.overlap参数调整,precursor mz
        public float oMzEnd;
        //前体的荷质比窗口
        public float wid;
        //原始文件中前体的窗口大小,未经过ExperimentDO.overlap参数调整,precursor mz
        public float oWid;
        //特定字段,在msLevel=0的时候使用,在Aird格式文件中使用,一个Swath块中所有MS2的rt时间列表
        public List<float> rts;
        //特定字段,在msLevel=0的时候使用,在压缩文件中存储mz数组的长度以及存储intensity数组的长度,mz长度及intensity长度交替存入
        public List<long> blocks;

    }
}

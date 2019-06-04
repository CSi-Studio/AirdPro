using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propro.Domains
{
    public class TempIndex
    {
        public int level;

        //retention time
        public float rt;

        //对应的ms1的序号
        public int pNum;
        //序号
        public int num;

        //前体的荷质比,precursor mz
        public float mz;

        //前体的荷质比窗口开始位置,已经经过ExperimentDO.overlap参数调整,precursor mz
        public float mzStart;

        //前体的荷质比窗口结束位置,已经经过ExperimentDO.overlap参数调整,precursor mz
        public float mzEnd;

        //前体的荷质比窗口
        public float wid;

        //用于存储KV键值对
        public string features;
    }
}

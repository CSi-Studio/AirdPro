namespace AirdPro.Domains.Aird
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
        public double mz;

        //前体的荷质比窗口开始位置,已经经过ExperimentDO.overlap参数调整,precursor mz
        public double mzStart;

        //前体的荷质比窗口结束位置,已经经过ExperimentDO.overlap参数调整,precursor mz
        public double mzEnd;

        //前体的荷质比窗口
        public double wid;

        //用于存储KV键值对
        public string features;
    }
}

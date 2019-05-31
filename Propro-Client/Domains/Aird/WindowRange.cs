

namespace Propro.Structs
{
    public class WindowRange
    {
        //前体的荷质比窗口开始位置,已经经过ExperimentDO.overlap参数调整,precursor mz
        public float start;

        //前体的荷质比窗口结束位置,已经经过ExperimentDO.overlap参数调整,precursor mz
        public float end;

        //前体的荷质比,precursor mz
        public float mz;

        public string features;

        public WindowRange() { }

        public WindowRange(float start, float end, float mz)
        {
            this.start = start;
            this.end = end;
            this.mz = mz;
        }
    }
}

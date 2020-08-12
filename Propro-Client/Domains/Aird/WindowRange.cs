namespace AirdPro.Domains.Aird
{
    public class WindowRange
    {
        //前体的荷质比窗口开始位置,已经经过ExperimentDO.overlap参数调整,precursor mz
        public double start;

        //前体的荷质比窗口结束位置,已经经过ExperimentDO.overlap参数调整,precursor mz
        public double end;

        //前体的荷质比,precursor mz
        public double mz;

        public string features;

        public WindowRange() { }

        public WindowRange(double start, double end, double mz)
        {
            this.start = start;
            this.end = end;
            this.mz = mz;
        }
    }
}

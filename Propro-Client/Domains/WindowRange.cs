

namespace Propro.Structs
{
    class WindowRange
    {
        //mz start
        public float start;
        
        //mz end
        public float end;

        //实际的precursor mz
        public float mz;

        public float interval;

        public WindowRange() { }

        public WindowRange(float start, float end, float mz)
        {
            this.start = start;
            this.end = end;
            this.mz = mz;
            this.interval = end - start;
        }
    }
}

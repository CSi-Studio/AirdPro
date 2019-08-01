using System;
using System.Collections;

namespace Propro_Client.Domains.Aird
{
    public class TempScan:IComparable
    {
        public int num;
        public float rt;
        public byte[] mzArrayBytes;
        public byte[] intArrayBytes;

        public TempScan(int num, float rt)
        {
            this.num = num;
            this.rt = rt;
        }

        public TempScan(int num, float rt, byte[] mzArrayBytes, byte[] intArrayBytes)
        {
            this.num = num;
            this.rt = rt;
            this.mzArrayBytes = mzArrayBytes;
            this.intArrayBytes = intArrayBytes;
        }

        public int CompareTo(object obj)
        {
            TempScan ts = (TempScan) obj;
            return this.rt.CompareTo(ts.rt);
        }
    }
}

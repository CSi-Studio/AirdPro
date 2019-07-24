namespace Propro_Client.Domains.Aird
{
    public class TempScan
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
    }
}

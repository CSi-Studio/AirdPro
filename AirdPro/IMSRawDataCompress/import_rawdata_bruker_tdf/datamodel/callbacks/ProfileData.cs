using System;
using System.Runtime.InteropServices;

namespace AirdPro.IMSRawDataCompress.datamodel.callbacks
{
    public class ProfileData : ProfileCallback
    {
        public long id;
        public long num_points;
        public int[] intensities;
        public IntPtr userData;

        public ProfileData()
        {
            intensities = null;
        }

        public void Invoke(long id, long numPoints, IntPtr intensityValues, IntPtr userData)
        {
            this.id = id;
            this.num_points = numPoints;

            // 确保numPoints不会导致溢出
            int safeNumPoints = (int)Math.Min(numPoints, Int32.MaxValue);

            // 将指向的非托管内存中的值复制到托管数组
            this.intensities = new int[safeNumPoints];
            Marshal.Copy(intensityValues, this.intensities, 0, safeNumPoints);

            this.userData = userData;
        }
    }
}

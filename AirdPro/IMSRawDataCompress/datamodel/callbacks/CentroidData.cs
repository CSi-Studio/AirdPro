using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.IMSRawDataCompress.datamodel.callbacks
{
    public class CentroidData : CentroidCallback
    {
        public long PrecursorId { get; private set; }
        public int NumPeaks { get; private set; }
        public double[] Mzs { get; private set; }
        public float[] Intensities { get; private set; }
        public void Invoke(long precursorId, int numPeaks, IntPtr pMz, IntPtr pIntensities, IntPtr userData)
        {
            PrecursorId = precursorId;
            NumPeaks = numPeaks;

            if (numPeaks != 0)
            {
                // 为mz数组分配内存
                Mzs = new double[numPeaks];
                // 从非托管内存复制数据到托管数组
                Marshal.Copy(pMz, Mzs, 0, numPeaks);

                // 为强度数组分配内存
                Intensities = new float[numPeaks];
                // 从非托管内存复制数据到托管数组
                Marshal.Copy(pIntensities, Intensities, 0, numPeaks);
            }
            else
            {
                // 如果没有峰值，则使用空数组
                Mzs = Array.Empty<double>();
                Intensities = Array.Empty<float>();
            }
        }
    }
}

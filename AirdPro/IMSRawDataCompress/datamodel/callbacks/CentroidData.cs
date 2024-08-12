using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.IMSRawDataCompress.datamodel.callbacks
{
    public class CentroidData : ICentroidCallback
    {
        public long PrecursorId { get; private set; }
        public int NumPeaks { get; private set; }
        public double[] Mzs { get; private set; }
        public float[] Intensities { get; private set; }
        public void Invoke(long precursorId, int numPeaks, IntPtr pMz, IntPtr pIntensities, IntPtr userData)
        {
            PrecursorId = precursorId;
            NumPeaks = numPeaks;

            if (pMz == IntPtr.Zero || pIntensities == IntPtr.Zero)
            {
                throw new InvalidOperationException("Pointer (pMz or pIntensities) is invalid.");
            }

            if (numPeaks != 0)
            {
                // 为mz数组分配内存
                Mzs = new double[numPeaks];
                // 从非托管内存复制数据到托管数组
                //Marshal.Copy(pMz, Mzs, 0, numPeaks);
                for (int i = 0; i < numPeaks; i++)
                {
                    // 计算每个元素的起始地址
                    IntPtr elementPtr = new IntPtr(pMz.ToInt64() + i * sizeof(double));
                    // 将指针指向的数据结构化为double类型并存储到数组中
                    Mzs[i] = (double)Marshal.PtrToStructure(elementPtr, typeof(double));
                }

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

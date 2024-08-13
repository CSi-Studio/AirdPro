using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace AirdPro.IMSRawDataCompress.datamodel.callbacks
{
    public class MultipleCentroidData : CentroidCallback
    {
        // 存储质谱数据点的内部类
        public class CentroidDataPoints
        {
            public long PrecursorId { get; set; }
            public double[] Mzs { get; set; }
            public float[] Intensities { get; set; }

            // 将质谱数据点转换为 DataPoint 数组
            public DataPoint[] ToDataPoints()
            {
                DataPoint[] dps = new DataPoint[Mzs.Length];
                for (int i = 0; i < Mzs.Length; i++)
                {
                    dps[i] = new DataPoint(Mzs[i], Intensities[i]);
                }
                return dps;
            }
        }

        // 使用字典来存储前体离子ID和对应的质谱数据点
        public Dictionary<long, CentroidDataPoints> MsmsSpectra { get; private set; }

        public MultipleCentroidData()
        {
            MsmsSpectra = new Dictionary<long, CentroidDataPoints>();
        }

        public void Invoke(long precursorId, int numPeaks, IntPtr pMz, IntPtr pIntensities, IntPtr userData)
        {
            // 为质谱数据点创建存储空间
            CentroidDataPoints centroidDataPoints = new CentroidDataPoints
            {
                PrecursorId = precursorId,
                Mzs = new double[numPeaks],
                Intensities = new float[numPeaks]
            };

            // 从非托管内存复制mz数组
            Marshal.Copy(pMz, centroidDataPoints.Mzs, 0, numPeaks);
            // 从非托管内存复制强度数组
            Marshal.Copy(pIntensities, centroidDataPoints.Intensities, 0, numPeaks);

            // 将数据点添加到字典中
            MsmsSpectra.Add(precursorId, centroidDataPoints);
        }
    }
}

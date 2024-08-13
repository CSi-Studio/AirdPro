using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace AirdPro.IMSRawDataCompress.datamodel.callbacks
{
    public class MultipleProfileData : ProfileCallback
    {
        // 使用Dictionary来存储谱图数据
        public Dictionary<long, ProfileDataPoints> Spectra { get; private set; }

        public class ProfileDataPoints
        {
            public long PrecursorId;
            public long NumPoints;
            public float[] Intensities;
            public IntPtr UserData;
        }

        public MultipleProfileData()
        {
            Spectra = new Dictionary<long, ProfileDataPoints>();
        }

        public void Invoke(long id, long numPoints, IntPtr intensityValues, IntPtr userData)
        {
            // 检查intensityValues是否为有效指针
            if (intensityValues == IntPtr.Zero)
                throw new ArgumentException("Invalid pointer for intensity values.");

            // 创建ProfileDataPoints实例
            ProfileDataPoints msMsSpectrum = new ProfileDataPoints
            {
                PrecursorId = id,
                NumPoints = numPoints
            };

            // 确保numPoints不会导致溢出
            int safeNumPoints = (int)Math.Min(numPoints, Int32.MaxValue);

            // 分配一个数组来存储强度数据
            msMsSpectrum.Intensities = new float[safeNumPoints];

            // 从非托管内存复制数据到托管数组
            Marshal.Copy(intensityValues, msMsSpectrum.Intensities, 0, safeNumPoints);

            msMsSpectrum.UserData = userData;

            // 添加到字典
            Spectra.Add(id, msMsSpectrum);
        }
    }
}

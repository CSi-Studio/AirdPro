using System;

namespace AirdPro.IMSRawDataCompress.datamodel.callbacks
{
    public interface IProfileCallback
    {
        void Invoke(long id, long numPoints, IntPtr intensityValues, IntPtr userData);
    }
}

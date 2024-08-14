using System;

namespace AirdPro.IMSRawDataCompress.datamodel.callbacks
{
    public interface ProfileCallback
    {
        void Invoke(long id, long numPoints, IntPtr intensityValues, IntPtr userData);
    }
}

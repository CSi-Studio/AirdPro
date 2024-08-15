using System;

namespace AirdPro.IMSRawDataCompress.datamodel.callbacks
{
    public interface ICentroidCallback
    {
        void Invoke(long precursorId, int numPeaks, IntPtr pMz, IntPtr pIntensities, IntPtr userData);
    }
}

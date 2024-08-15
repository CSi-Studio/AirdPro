using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.IMSRawDataCompress.datamodel.callbacks
{
    public interface ICentroidCallback
    {
        void Invoke(long precursorId, int numPeaks, IntPtr pMz, IntPtr pIntensities, IntPtr userData);
    }
}

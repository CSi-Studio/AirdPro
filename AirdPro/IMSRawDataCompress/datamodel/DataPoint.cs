using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.IMSRawDataCompress.datamodel
{
    public class DataPoint
    {
        public double Mz { get; }
        public float Intensity { get; }

        public DataPoint(double mz, float intensity)
        {
            Mz = mz;
            Intensity = intensity;
        }
    }
}

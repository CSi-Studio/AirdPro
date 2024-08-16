using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.IMSRawDataCompress.datamodel
{
    public class SpectrumData
    {
        public double Mz { get; set; }
        public double Intensity { get; set; }
        public double Mobility { get; set; }

        public SpectrumData(double mz, double intensity, double mobility)
        {
            Mz = mz;
            Intensity = intensity;
            Mobility = mobility;
        }
    }
}

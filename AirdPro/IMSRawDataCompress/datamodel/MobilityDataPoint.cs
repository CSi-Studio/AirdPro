using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.IMSRawDataCompress.datamodel
{
    public class MobilityDataPoint : DataPoint
    {
        public double mz { get; }
        public double intensity { get; }
        public double mobility{get; }
        public int scanNum { get; }

        public MobilityDataPoint(double mz, double intensity, double mobility, int scanNum)
        {
            this.mz = mz;
            this.intensity = intensity;
            this.mobility = mobility;
            this.scanNum = scanNum;
        }
    }
}

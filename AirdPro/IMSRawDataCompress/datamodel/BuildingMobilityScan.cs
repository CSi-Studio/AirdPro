using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.IMSRawDataCompress.datamodel
{
    public class BuildingMobilityScan
    {
        public int ScanNumber { get; set; }
        public double[] IntensityValues { get; set; }
        public double[] MzValues { get; set; }
        public int BasePeakIndex { get; set; } //intensityValues数组中最大值的index

        /**
          * @param scanNumber  The scan number beginning with 0
          * @param mzs         The m/z values
          * @param intensities The intensity values.
          */
        public BuildingMobilityScan(int scanNumber, double[] mzs, double[] intensities)
        {
            if (intensities.Length != mzs.Length)
            {
                throw new ArgumentException("Construct BuildingMobilityScan failed: intensities.Length != mzs.Length");
            }

            this.ScanNumber = scanNumber;
            this.BasePeakIndex = -1;
            if (mzs.Length >= 1)
            {
                this.BasePeakIndex = 0;
                for (int i = 1; i < mzs.Length; i++)
                {
                    if (intensities[i] > intensities[this.BasePeakIndex])
                    {
                        this.BasePeakIndex = i;
                    }
                }
            }

            this.IntensityValues = intensities;
            this.MzValues = mzs;
        }
    }
}

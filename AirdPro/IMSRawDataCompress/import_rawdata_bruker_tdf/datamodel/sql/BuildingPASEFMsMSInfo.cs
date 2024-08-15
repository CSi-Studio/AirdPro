using AirdPro.IMSRawDataCompress.datamodel;
using CSharpFastPFOR.Port;

namespace AirdPro.IMSRawDataCompress.import_rawdata_bruker_tdf.datamodel.sql
{
    public class BuildingPASEFMsMsInfo
    {
        private readonly double precursorMz;
        private readonly Range<long> spectrumNumberRange;
        private readonly double collisionEnergy;
        private readonly long precursorCharge;
        private readonly long parentFrameNumber;
        private readonly long fragmentFrameNumber;
        private readonly double isolationWidth;

        public BuildingPASEFMsMsInfo(double precursorMz, Range<long> spectrumNumberRange,
      double collisionEnergy, long precursorCharge, long parentFrameNumber, long fragmentFrameNumber,
      double isolationWidth)
        {
            this.precursorMz = precursorMz;
            this.spectrumNumberRange = spectrumNumberRange;
            this.collisionEnergy = collisionEnergy;
            this.precursorCharge = precursorCharge;
            this.parentFrameNumber = parentFrameNumber;
            this.fragmentFrameNumber = fragmentFrameNumber;
            this.isolationWidth = isolationWidth;
        }

    }
}

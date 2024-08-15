using AirdPro.IMSRawDataCompress.datamodel;

namespace AirdPro.IMSRawDataCompress.import_rawdata_bruker_tdf.datamodel.sql
{
    public class BuildingPASEFMsMsInfo
    {
        private readonly double precursorMz;
        private readonly Range<int> spectrumNumberRange;
        private readonly float collisionEnergy;
        private readonly int precursorCharge;
        private readonly int parentFrameNumber;
        private readonly int fragmentFrameNumber;
        private readonly double isolationWidth;
    }
}

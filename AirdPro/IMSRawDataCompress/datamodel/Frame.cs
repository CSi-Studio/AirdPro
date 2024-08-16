using AirdPro.IMSRawDataCompress.datamodel.callbacks;

namespace AirdPro.IMSRawDataCompress.datamodel
{
    public class Frame
    {
        public long frameId;
        public double[] mzArray { get; set; }
        public float[] intensityArray { get; set; }
        public double[] mobilityArray { get; set; }
        public CentroidData centroidData { get; set; }

        public Frame(long frameId)
        {
            this.frameId = frameId;
        }

        public Frame(long frameId, CentroidData centroidData)
        {
            this.frameId = frameId;
            this.centroidData = centroidData;
        }

        public long FrameId { get => frameId; }        

        public CentroidData CentroidData { get => centroidData; }
    }

}

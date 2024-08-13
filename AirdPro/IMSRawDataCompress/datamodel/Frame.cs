using AirdPro.IMSRawDataCompress.datamodel.callbacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.IMSRawDataCompress.datamodel
{
    public class Frame
    {
        private int frameId;
        private CentroidData centroidData;

        public Frame(int frameId, CentroidData centroidData)
        {
            this.frameId = frameId;
            this.centroidData = centroidData;
        }

        public int FrameId { get => frameId; }
        public CentroidData CentroidData { get => centroidData; }
    }
}

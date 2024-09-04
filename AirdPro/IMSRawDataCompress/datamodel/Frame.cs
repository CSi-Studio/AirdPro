using AirdPro.Domains;
using AirdPro.IMSRawDataCompress.datamodel.callbacks;
using AirdPro.IMSRawDataCompress.datamodel.enums;
using System;
using System.Collections.Generic;
namespace AirdPro.IMSRawDataCompress.datamodel
{ 
    public class Frame
    {
        public MobilityType mobilityType { get; set; }
        public HashSet<PasefMsMsInfo> precursorInfos { get; set; }
        public Range<double> mobilityRange { get; set; }
        public int mobilitySegment = -1;

        public long frameId;
        public double[] mzArray { get; set; }
        public float[] intensityArray { get; set; }
        public double[] mobilityArray { get; set; }
        public CentroidData centroidData { get; set; }
        public List<BuildingMobilityScan> mobilityScans { get; set; }

        public Frame(long scanNumber, long msLevel,
            double retentionTime, double[] mzValues, double[] intensityValues,
            MassSpectrumType spectrumType, PolarityType polarity, string scanDefinition,
            Range<double> scanMZRange, MobilityType mobilityType,
            HashSet<PasefMsMsInfo> precursorInfos, double accumulationTime)
        {
            this.mobilityType = mobilityType;
            mobilityRange = Range<double>.Singleton(0.0);
            this.precursorInfos = precursorInfos ?? new HashSet<PasefMsMsInfo>();
        }        

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

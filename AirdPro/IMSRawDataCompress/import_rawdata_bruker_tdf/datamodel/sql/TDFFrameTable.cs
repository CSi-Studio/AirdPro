using System;
using System.Linq;

namespace AirdPro.IMSRawDataCompress.import_rawdata_bruker_tdf.datamodel.sql
{
    public class TDFFrameTable : TDFDataTable
    {
        public const string FRAME_TABLE_NAME = "Frames";

        public const string FRAME_ID = "Id";
        public const string TIME = "Time";
        public const string POLARITY = "Polarity";
        public const string SCAN_MODE = "ScanMode";
        public const string MSMS_TYPE = "MsMsType";
        public const string TIMS_ID = "TimsId";
        public const string MAX_INTENSITY = "MaxIntensity";
        public const string SUMMED_INTENSITIES = "SummedIntensities";
        public const string NUM_SCANS = "NumScans";
        public const string NUM_PEAKS = "NumPeaks";
        public const string MZ_CALIBRATION = "MzCalibration";
        public const string T1 = "T1";
        public const string T2 = "T2";
        public const string TIMS_CALIBRATION = "TimsCalibration";
        public const string PROPERTY_GROUP = "PropertyGroup";
        public const string ACCUMULATION_TIME = "AccumulationTime";
        public const string RAMP_TIME = "RampTime";

        private readonly TDFDataColumn<long> frameIdColumn;
        private readonly TDFDataColumn<double> timeColumn;
        private readonly TDFDataColumn<string> polarityColumn;
        private readonly TDFDataColumn<long> scanModeColumn;
        private readonly TDFDataColumn<long> msMsTypeColumn;
        private readonly TDFDataColumn<long> timsIdColumn;
        private readonly TDFDataColumn<long> maxIntensityColumn;
        private readonly TDFDataColumn<long> summedIntensityColumn;
        private readonly TDFDataColumn<long> numScansColumn;
        private readonly TDFDataColumn<long> numPeaksColumn;
        private readonly TDFDataColumn<long> mzCalibrationColumn;
        private readonly TDFDataColumn<double> t1Column;
        private readonly TDFDataColumn<double> t2Column;
        private readonly TDFDataColumn<long> timsCalibrationColumn;
        private readonly TDFDataColumn<long> propertyGroupColumn;
        private readonly TDFDataColumn<double> accumulationTimeColumn;
        private readonly TDFDataColumn<double> rampTimeColumn;

        public TDFFrameTable() : base(FRAME_TABLE_NAME)
        {
            frameIdColumn = new TDFDataColumn<long>(FRAME_ID);
            base.AddKeyColumn(frameIdColumn);
            timeColumn = new TDFDataColumn<double>(TIME);
            base.AddColumn(timeColumn);
            polarityColumn = new TDFDataColumn<string>(POLARITY);
            base.AddColumn(polarityColumn);
            scanModeColumn = new TDFDataColumn<long>(SCAN_MODE);
            base.AddColumn(scanModeColumn);
            msMsTypeColumn = new TDFDataColumn<long>(MSMS_TYPE);
            base.AddColumn(msMsTypeColumn);
            timsIdColumn = new TDFDataColumn<long>(TIMS_ID);
            base.AddColumn(timsIdColumn);
            maxIntensityColumn = new TDFDataColumn<long>(MAX_INTENSITY);
            base.AddColumn(maxIntensityColumn);
            summedIntensityColumn = new TDFDataColumn<long>(SUMMED_INTENSITIES);
            base.AddColumn(summedIntensityColumn);
            numScansColumn = new TDFDataColumn<long>(NUM_SCANS);
            base.AddColumn(numScansColumn);
            numPeaksColumn = new TDFDataColumn<long>(NUM_PEAKS);
            base.AddColumn(numPeaksColumn);
            mzCalibrationColumn = new TDFDataColumn<long>(MZ_CALIBRATION);
            base.AddColumn(mzCalibrationColumn);
            t1Column = new TDFDataColumn<double>(T1);
            base.AddColumn(t1Column);
            t2Column = new TDFDataColumn<double>(T2);
            base.AddColumn(t2Column);
            timsCalibrationColumn = new TDFDataColumn<long>(TIMS_CALIBRATION);
            base.AddColumn(timsCalibrationColumn);
            propertyGroupColumn = new TDFDataColumn<long>(PROPERTY_GROUP);
            base.AddColumn(propertyGroupColumn);
            accumulationTimeColumn = new TDFDataColumn<double>(ACCUMULATION_TIME);
            base.AddColumn(accumulationTimeColumn);
            rampTimeColumn = new TDFDataColumn<double>(RAMP_TIME);
            base.AddColumn(rampTimeColumn);
        }

        public long FirstFrameId()
        {
            return GetFrameIdColumn().GetValueList().First();
        }

        public long LastFrameId()
        {
            return GetFrameIdColumn().GetValueList().Last();
        }

        public TDFDataColumn<long> GetFrameIdColumn()
        {
            return frameIdColumn;
        }

        public TDFDataColumn<double> GetTimeColumn()
        {
            return timeColumn;
        }

        public TDFDataColumn<string> GetPolarityColumn()
        {
            return polarityColumn;
        }

        public TDFDataColumn<long> GetScanModeColumn()
        {
            return scanModeColumn;
        }

        public TDFDataColumn<long> GetMsMsTypeColumn()
        {
            return msMsTypeColumn;
        }

        public TDFDataColumn<long> GetTimsIdColumn()
        {
            return timsIdColumn;
        }

        public TDFDataColumn<long> GetMaxIntensityColumn()
        {
            return maxIntensityColumn;
        }

        public TDFDataColumn<long> GetSummedIntensityColumn()
        {
            return summedIntensityColumn;
        }

        public TDFDataColumn<long> GetNumScansColumn()
        {
            return numScansColumn;
        }

        public TDFDataColumn<long> GetNumPeaksColumn()
        {
            return numPeaksColumn;
        }

        public TDFDataColumn<long> GetMzCalibrationColumn()
        {
            return mzCalibrationColumn;
        }

        public TDFDataColumn<double> GetT1Column()
        {
            return t1Column;
        }

        public TDFDataColumn<double> GetT2Column()
        {
            return t2Column;
        }

        public TDFDataColumn<long> GetTimsCalibrationColumn()
        {
            return timsCalibrationColumn;
        }

        public TDFDataColumn<long> GetPropertyGroupColumn()
        {
            return propertyGroupColumn;
        }

        public TDFDataColumn<double> GetAccumulationTimeColumn()
        {
            return accumulationTimeColumn;
        }

        public TDFDataColumn<double> GetRampTimeColumn()
        {
            return rampTimeColumn;
        }
    }
}

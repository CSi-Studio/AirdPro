using AirdPro.IMSRawDataCompress.datamodel;
using AirdPro.IMSRawDataCompress.datamodel.msms;

namespace AirdPro.IMSRawDataCompress.import_rawdata_bruker_tdf.datamodel.sql
{
    /**
     * Additional MS/MS meta-information.
     */
    public class TDFFrameMsMsInfoTable : TDFDataTable
    {
        public const string FRAME_MSMS_INFO_TABLE = "FrameMsMsInfo";

        public const string FRAME_ID = "Frame";
        public const string PARENT_ID = "Parent";
        public const string TRIGGER_MASS = "TriggerMass";
        public const string ISOLATION_WIDTH = "IsolationWidth";
        public const string PRECURSOR_CHARGE = "PrecursorCharge";
        public const string COLLISION_ENERGY = "CollisionEnergy";

        private readonly TDFDataColumn<long> frameIdColumn;
        private readonly TDFDataColumn<long> parentIdColumn;
        private readonly TDFDataColumn<double> precursorMzColumn;
        private readonly TDFDataColumn<double> isolationWidthColumn;
        private readonly TDFDataColumn<long> chargeColumn;
        private readonly TDFDataColumn<double> ceColumn;

        public TDFFrameMsMsInfoTable() : base(FRAME_MSMS_INFO_TABLE)
        {
            frameIdColumn = new TDFDataColumn<long>(FRAME_ID);
            base.AddKeyColumn(frameIdColumn);
            parentIdColumn = new TDFDataColumn<long>(PARENT_ID);
            base.AddColumn(parentIdColumn);
            precursorMzColumn = new TDFDataColumn<double>(TRIGGER_MASS);
            base.AddColumn(precursorMzColumn);
            isolationWidthColumn = new TDFDataColumn<double>(ISOLATION_WIDTH);
            base.AddColumn(isolationWidthColumn);
            chargeColumn = new TDFDataColumn<long>(PRECURSOR_CHARGE);
            base.AddColumn(chargeColumn);
            ceColumn = new TDFDataColumn<double>(COLLISION_ENERGY);
            base.AddColumn(ceColumn);
        }        

        public IDDAMsMsInfo GetDDAMsMsInfo(int index, int msLevel, Scan msmsScan, Scan parentScan)
        {
            double precursor = precursorMzColumn.GetValueList()[index];
            double width = isolationWidthColumn.GetValueList()[index];
            DDAMsMsInfo ddaMsMsInfo = new DDAMsMsInfo(precursor, chargeColumn.GetValueList()[index],
                ceColumn.GetValueList()[index], msmsScan, parentScan, msLevel, ActivationMethod.CID,
                Range<double>.Closed(precursor - width / 2, precursor + width / 2));
            return ddaMsMsInfo;
        }

    }
}

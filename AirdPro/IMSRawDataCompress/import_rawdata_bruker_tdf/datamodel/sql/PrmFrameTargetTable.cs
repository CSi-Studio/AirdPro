
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AirdPro.IMSRawDataCompress.import_rawdata_bruker_tdf.datamodel.sql
{
    public class PrmFrameTargetTable :TDFDataTable
    {
        public const string PRM_FRAME_PRECURSOR_TABLE = "PrmFramePrecursorTable";

        private readonly TDFDataColumn<long> frameIdColumn;
        private readonly TDFDataColumn<long> targetIdColumn;
        private readonly TDFDataColumn<long> scanNumBeginColumn;
        private readonly TDFDataColumn<long> scanNumEndColumn;
        private readonly TDFDataColumn<double> collisionEnergyColumn;
        private readonly TDFDataColumn<double> isolationWidthColumn;
        private readonly TDFDataColumn<double> isolationMzColumn;
        private readonly TDFDataColumn<long> chargeColumn;

        private readonly Dictionary<int, HashSet<BuildingPASEFMsMsInfo>> info;

        public PrmFrameTargetTable() : base(PRM_FRAME_PRECURSOR_TABLE)
        {
            frameIdColumn = new TDFDataColumn<long>(TDFPrmFrameMsMsInfoTable.FRAME_ID);
            base.AddKeyColumn(frameIdColumn);
            targetIdColumn = new TDFDataColumn<long>(TDFPrmFrameMsMsInfoTable.TARGET);
            base.AddColumn(targetIdColumn);
            scanNumBeginColumn = new TDFDataColumn<long>(TDFPrmFrameMsMsInfoTable.SCAN_NUM_BEGIN);
            base.AddColumn(scanNumBeginColumn);
            scanNumEndColumn = new TDFDataColumn<long>(TDFPrmFrameMsMsInfoTable.SCAN_NUM_END);
            base.AddColumn(scanNumEndColumn);
            collisionEnergyColumn = new TDFDataColumn<double>(TDFPrmFrameMsMsInfoTable.COLLISION_ENERGY);
            base.AddColumn(collisionEnergyColumn);
            isolationWidthColumn = new TDFDataColumn<double>(TDFPrmFrameMsMsInfoTable.ISOLATION_WIDTH);
            base.AddColumn(isolationWidthColumn);
            isolationMzColumn = new TDFDataColumn<double>(TDFPrmFrameMsMsInfoTable.ISOLATION_MZ);
            base.AddColumn(isolationMzColumn);
            chargeColumn = new TDFDataColumn<long>(TDFPrmTargetsTable.CHARGE);
            base.AddColumn(chargeColumn);

            info = new ();
        }
    }
}

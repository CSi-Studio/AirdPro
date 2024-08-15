using Aga.Controls.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace AirdPro.IMSRawDataCompress.import_rawdata_bruker_tdf.datamodel.sql
{
    public class TDFPrmFrameMsMsInfoTable :TDFDataTable
    {
        public const string PRM_FRAME_MSMS_TABLE_NAME = "PrmFrameMsMsInfo";

        public const string FRAME_ID = "Frame";
        public const string SCAN_NUM_BEGIN = "ScanNumBegin";
        public const string SCAN_NUM_END = "ScanNumEnd";
        public const string ISOLATION_MZ = "IsolationMz";
        public const string ISOLATION_WIDTH = "IsolationWidth";
        public const string COLLISION_ENERGY = "CollisionEnergy";
        public const string TARGET = "Target";

        private readonly TDFDataColumn<long> frameIdColumn;
        private readonly TDFDataColumn<long> targetIdColumn;
        private readonly TDFDataColumn<long> scanNumBeginColumn;
        private readonly TDFDataColumn<long> scanNumEndColumn;
        private readonly TDFDataColumn<double> collisionEnergyColumn;
        private readonly TDFDataColumn<double> isolationWidthColumn;
        private readonly TDFDataColumn<double> isolationMzColumn;

        public TDFPrmFrameMsMsInfoTable() : base(PRM_FRAME_MSMS_TABLE_NAME)
        {
            frameIdColumn = new TDFDataColumn<long>(FRAME_ID);
            base.AddKeyColumn(frameIdColumn);
            targetIdColumn = new TDFDataColumn<long>(TARGET);
            base.AddColumn(targetIdColumn);
            scanNumBeginColumn = new TDFDataColumn<long>(SCAN_NUM_BEGIN);
            base.AddColumn(scanNumBeginColumn);
            scanNumEndColumn = new TDFDataColumn<long>(SCAN_NUM_END);
            base.AddColumn(scanNumEndColumn);
            collisionEnergyColumn = new TDFDataColumn<double>(COLLISION_ENERGY);
            base.AddColumn(collisionEnergyColumn);
            isolationWidthColumn = new TDFDataColumn<double>(ISOLATION_WIDTH);
            base.AddColumn(isolationWidthColumn);
            isolationMzColumn = new TDFDataColumn<double>(ISOLATION_MZ);
            base.AddColumn(isolationMzColumn);
        }
    }
}

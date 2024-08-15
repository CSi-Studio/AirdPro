using System;
using System.Collections.Generic;

namespace AirdPro.IMSRawDataCompress.import_rawdata_bruker_tdf.datamodel.sql
{
    public class FramePrecursorTable : TDFDataTable
    {
        public const String FRAME_PRECURSOR_TABLE = "FramePrecursorTable";
             
        
        private readonly TDFDataColumn<long> frameIdColumn;
        private readonly TDFDataColumn<long> precursorIdColumn;
        private readonly TDFDataColumn<long> scanNumBeginColumn;
        private readonly TDFDataColumn<long> scanNumEndColumn;
        private readonly TDFDataColumn<double> collisionEnergyColumn;
        private readonly TDFDataColumn<double> isolationWidthColumn;
        private readonly TDFDataColumn<double> largestPeakMzColumn;
        private readonly TDFDataColumn<double> isolationMzColumn;
        private readonly TDFDataColumn<long> chargeColumn;
        private readonly TDFDataColumn<long> parentIdColumn;

        /**
        * Key = FrameId of the MS2 Frame
        * <p></p>
        * Value = Collection of PasefMsMsInfo on all precursors in the frame.
        */
        private Dictionary<int, HashSet<BuildingPASEFMsMsInfo>> info;

        public FramePrecursorTable() : base(FRAME_PRECURSOR_TABLE)
        {
            // added by constructor            
            frameIdColumn = (TDFDataColumn<long>)GetColumn(TDFPasefFrameMsMsInfoTable.FRAME_ID);

            // add manually
           /* precursorIdColumn = new TDFDataColumn<long>(PRECURSOR_ID);
            base.AddKeyColumn(precursorIdColumn);
            scanNumBeginColumn = new TDFDataColumn<long>(SCAN_NUM_BEGIN);
            base.AddKeyColumn(scanNumBeginColumn);
            scanNumEndColumn = new TDFDataColumn<long>(SCAN_NUM_END);
            base.AddKeyColumn(scanNumEndColumn);
            collisionEnergyColumn = new TDFDataColumn<long>(COLLISION_ENERGY);
            base.AddKeyColumn(collisionEnergyColumn);
            isolationMzColumn = new TDFDataColumn<long>(ISOLATION_MZ);
            base.AddKeyColumn(isolationMzColumn);
            isolationWidthColumn = new TDFDataColumn<long>(ISOLATION_WIDTH);
            base.AddKeyColumn(isolationWidthColumn);
            largestPeakMzColumn = new TDFDataColumn<long>(LARGEST_PEAK_MZ);
            base.AddKeyColumn(largestPeakMzColumn);
            chargeColumn = new TDFDataColumn<long>(CHARGE);
            base.AddKeyColumn(chargeColumn);
            parentIdColumn = new TDFDataColumn<long>(PARENT_ID);
            base.AddKeyColumn(parentIdColumn);*/
        }

    }
}

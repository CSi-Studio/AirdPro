using System.Collections.Generic;

namespace AirdPro.IMSRawDataCompress.import_rawdata_bruker_tdf.datamodel.sql
{
    public class TDFPasefFrameMsMsInfoTable : TDFDataTable
    {
        public const string PASEF_FRAME_MSMS_TABLE_NAME = "PasefFrameMsMsInfo";

        public const string FRAME_ID = "Frame";
        public const string SCAN_NUM_BEGIN = "ScanNumBegin";
        public const string SCAN_NUM_END = "ScanNumEnd";
        public const string ISOLATION_MZ = "IsolationMz";
        public const string ISOLATION_WIDTH = "IsolationWidth";
        public const string COLLISION_ENERGY = "CollisionEnergy";
        public const string PRECURSOR_ID = "Precursor";

        private readonly TDFDataColumn<long> frameIdColumn;
        private readonly TDFDataColumn<long> precursorIdColumn;
        private readonly TDFDataColumn<long> scanNumBeginColumn;
        private readonly TDFDataColumn<long> scanNumEndColumn;

        public TDFPasefFrameMsMsInfoTable() : base(PASEF_FRAME_MSMS_TABLE_NAME)
        {
            // added by constructor
            frameIdColumn = (TDFDataColumn<long>)GetColumn(TDFPasefFrameMsMsInfoTable.FRAME_ID);

            // add manually
            precursorIdColumn = new TDFDataColumn<long>(PRECURSOR_ID);
            base.AddColumn(precursorIdColumn);
            scanNumBeginColumn = new TDFDataColumn<long>(SCAN_NUM_BEGIN);
            base.AddColumn(scanNumBeginColumn);
            scanNumEndColumn = new TDFDataColumn<long>(SCAN_NUM_END);
            base.AddColumn(scanNumEndColumn);
        }

        /**
        *
        * @param frame
        * @param brukerScanNum Bruker layout!
        * @return the precursor id or -1;
        */
        public long getPrecursorIdAtScan(long frame, long brukerScanNum)
        {
           List<long> frameIdList = frameIdColumn.GetValueList();
            int index = 0;

            for (; index < frameIdList.Count; index++)
            {
                if (frameIdList[index] == frame
                    && scanNumBeginColumn.GetValueList()[index] <= brukerScanNum
                    && scanNumEndColumn.GetValueList()[index] > brukerScanNum)
                {
                    return precursorIdColumn.GetValueList()[index];
                }
                else if (frameIdColumn.GetValueList()[index] > frame)
                {
                    break;
                }
            }
            return -1;
        }
    }
}

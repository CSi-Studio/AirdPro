using AirdPro.Constants;
using Google.Protobuf.WellKnownTypes;
using StackExchange.Redis;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using System;
using CSharpFastPFOR.Port;
using System.Linq;
using System.Collections.Generic;

namespace AirdPro.IMSRawDataCompress.import_rawdata_bruker_tdf.datamodel.sql
{
    public class TDFPrecursorTable : TDFDataTable
    {
        public const string PRECURSOR_TABLE_NAME = "Precursors";

        public const string PRECURSOR_ID = "Id";
        public const string LARGEST_PEAK_MZ = "LargestPeakMz";
        public const string AVERAGE_MZ = "AverageMz";
        public const string MONOISOTOPIC_MZ = "MonoisotopicMz";
        public const string CHARGE = "Charge";
        public const string SCAN_NUMBER = "ScanNumber";
        public const string INTENSITY = "Intensity";
        public const string PARENT_ID = "Parent";

        private readonly TDFDataColumn<long> precursorIdColumn;
        private readonly TDFDataColumn<double> largestPeakMzColumn;
        private readonly TDFDataColumn<double> averageMzColumn;
        private readonly TDFDataColumn<double> monoisotopicMzColumn;
        private readonly TDFDataColumn<long> chargeColumn;
        private readonly TDFDataColumn<double> scanNumberColumn;
        private readonly TDFDataColumn<double> intensityColumn;
        private readonly TDFDataColumn<long> frameIdColumn;

        public TDFPrecursorTable() : base(PRECURSOR_TABLE_NAME)
        {
            precursorIdColumn = new TDFDataColumn<long>(PRECURSOR_ID);
            base.AddKeyColumn(precursorIdColumn);            
            largestPeakMzColumn = new TDFDataColumn<double>(LARGEST_PEAK_MZ);
            base.AddColumn(largestPeakMzColumn);
            averageMzColumn = new TDFDataColumn<double>(AVERAGE_MZ);
            base.AddColumn(averageMzColumn);
            monoisotopicMzColumn = new TDFDataColumn<double>(MONOISOTOPIC_MZ);
            base.AddColumn(monoisotopicMzColumn);
            chargeColumn = new TDFDataColumn<long>(CHARGE);
            base.AddColumn(chargeColumn);
            scanNumberColumn = new TDFDataColumn<double>(SCAN_NUMBER);
            base.AddColumn(scanNumberColumn);
            intensityColumn = new TDFDataColumn<double>(INTENSITY);
            base.AddColumn(intensityColumn);
            frameIdColumn = new TDFDataColumn<long>(PARENT_ID);
            base.AddColumn(frameIdColumn);
        }        

        public HashSet<long> GetPrecursorIdsForMS1Frame(long frame)
        {
            HashSet<long> precursorIds = new HashSet<long>();
            List<long> valueList = frameIdColumn.GetValueList();

            for (int index = 0; index < valueList.Count; index++)
            {
                if (valueList[index] == frame)
                {
                    precursorIds.Add(valueList[index]);
                }
                else if (valueList[index] > frame)
                {
                    break;
                }
            }
            return precursorIds;
        }

        

    }
}

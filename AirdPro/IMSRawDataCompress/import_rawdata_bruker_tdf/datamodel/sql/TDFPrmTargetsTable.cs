using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.IMSRawDataCompress.import_rawdata_bruker_tdf.datamodel.sql
{
    public class TDFPrmTargetsTable : TDFDataTable
    {
        public const string TARGET_TABLE_NAME = "PrmTargets";

        public const string TARGET_ID = "Id";
        public const string EXTERNAL_ID = "ExternalId";
        public const string TIME = "Time";
        public const string ONE_OVER_K0 = "OneOverK0";
        public const string MONOISOTOPIC_MZ = "MonoisotopicMz";
        public const string CHARGE = "Charge";
        public const string DESCRIPTION = "Description";

        private readonly TDFDataColumn<long> precursorIdColumn;
        private readonly TDFDataColumn<string> externalIdColumn;
        private readonly TDFDataColumn<double> timeColumn;
        private readonly TDFDataColumn<double> oneOverK0Column;
        private readonly TDFDataColumn<double> monoisotopicMzColumn;
        private readonly TDFDataColumn<long> chargeColumn;
        private readonly TDFDataColumn<string> descriptionColumn;

        public TDFPrmTargetsTable() : base(TARGET_TABLE_NAME)
        {
            precursorIdColumn = new TDFDataColumn<long>(TARGET_ID);
            base.AddKeyColumn(precursorIdColumn);
            externalIdColumn = new TDFDataColumn<string>(EXTERNAL_ID);
            base.AddColumn(externalIdColumn);
            timeColumn = new TDFDataColumn<double>(TIME);
            base.AddColumn(timeColumn);
            oneOverK0Column = new TDFDataColumn<double>(ONE_OVER_K0);
            base.AddColumn(oneOverK0Column);
            monoisotopicMzColumn = new TDFDataColumn<double>(MONOISOTOPIC_MZ);
            base.AddColumn(monoisotopicMzColumn);
            chargeColumn = new TDFDataColumn<long>(CHARGE);
            base.AddColumn(chargeColumn);
            descriptionColumn = new TDFDataColumn<string>(DESCRIPTION);
            base.AddColumn(descriptionColumn);
        }
    }
}

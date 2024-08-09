using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.IMSRawDataCompress.datamodel.sql
{
    public class TDFDataColumn<DataType> : List<DataType>
    {
        protected readonly string ColumnName;

        public TDFDataColumn(string columnName)
        {
            this.ColumnName = columnName ?? throw new ArgumentNullException(nameof(columnName));
        }

        public string GetColumnName()
        {
            return ColumnName;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.IMSRawDataCompress.import_rawdata_bruker_tdf.datamodel.sql
{
    public interface IDataColumn<out T>
    {
        string ColumnName { get; }
        IEnumerable<T> Values { get; } // 使用IEnumerable<T>来提供协变性
    }
}

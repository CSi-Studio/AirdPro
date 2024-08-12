using System;
using System.Collections.Generic;
using System.Linq;

namespace AirdPro.IMSRawDataCompress.datamodel.sql
{
    public interface IColumnVariant<out T>
    {
        string ColumnName { get; }
        IEnumerable<T> Values { get; } // 使用IEnumerable<T>来提供协变性
    }

    public class TDFDataColumn<T>  : IColumnVariant<Object> 
    {
        private string _columnName;
        private List<T> _valueList;

        public string ColumnName => _columnName;

        //Attention: cast保证TDFDataColumn<long>等值类型泛型参数可以正确转换为IEnumerable<Object>!!!
        public IEnumerable<Object> Values => _valueList.Cast<object>(); 

        public TDFDataColumn(string columnName)
        {
            this._columnName = columnName ?? throw new ArgumentNullException(nameof(columnName));
            this._valueList = new List<T>();
        }

        public string GetColumnName()
        {
            return _columnName;
        }

        public List<T> GetValueList()
        {
            return _valueList;
        }
    }
}

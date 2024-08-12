using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.IMSRawDataCompress.datamodel.sql
{
    public abstract class TDFDataTable
    {
        protected  string _tableName;
        protected  string _keyColumnName;
        protected  List<IColumnVariant<Object>> _columns;
        protected IColumnVariant<Object> _keyColumn;

        public String GetTableName() { return _tableName; }
        public String GetEntryHeader() { return _keyColumnName; }
        public String GetKeyColumnName() { return _keyColumnName; }
        public TDFDataTable(string tableName)
        {
            this._tableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            this._columns = new List<IColumnVariant<Object>>();
        }

        public List<IColumnVariant<Object>> GetColumns()
        {
            return _columns;
        }

        public IColumnVariant<Object> GetKeyColumn()
        {
            return _keyColumn;
        }

        public void AddKeyColumn(IColumnVariant<Object> column)
        {
            if (column == null)
                throw new ArgumentNullException(nameof(column));

            //Attention: put KeyColumn to first position of List!!!
            _columns.Insert(0, column);
            this._keyColumn = column;
            this._keyColumnName = column.ColumnName;
        }

        public void AddColumn(IColumnVariant<Object> column)
        {
            if (column == null)
                throw new ArgumentNullException(nameof(column));

            //put Column to last position of List
            _columns.Add(column);
        }

        public IColumnVariant<Object> GetColumnByName(string columnName)
        {
            foreach (var column in _columns)
            {
                if (column.ColumnName.Equals(columnName))
                    return column as TDFDataColumn<Object>;
            }
            return null;
        }

        protected string GetColumnHeadersForQuery()
        {
            var headers = new StringBuilder();
            foreach (var col in _columns)
            {
                headers.Append(col.ColumnName + ", ");
            }
            if (headers.Length > 0)
                headers.Remove(headers.Length - 2, 2);

            return headers.ToString();
        }

        public bool IsValid()
        {
            long numKeys = _keyColumn.Values.Count();
            foreach (var col in _columns)
            {
                if (numKeys != col.Values.Count())
                    return false;
            }
            return true;
        }

        public static SqlDbType GetSqlDbTypeFromType(Type type)
        {
            if (type == typeof(int))
                return SqlDbType.Int;
            else if (type == typeof(long))
                return SqlDbType.BigInt;
            else if (type == typeof(short))
                return SqlDbType.SmallInt;
            else if (type == typeof(byte))
                return SqlDbType.TinyInt;
            else if (type == typeof(float))
                return SqlDbType.Float;
            else if (type == typeof(double))
                return SqlDbType.Float;
            else if (type == typeof(decimal))
                return SqlDbType.Decimal;
            else if (type == typeof(string))
                return SqlDbType.Text;
            else if (type == typeof(char))
                return SqlDbType.NChar;
            else if (type == typeof(byte[]))
                return SqlDbType.VarBinary;
            else if (type == typeof(DateTime))
                return SqlDbType.DateTime;
            else if (type == typeof(DateTimeOffset))
                return SqlDbType.DateTimeOffset;
            else if (type == typeof(TimeSpan))
                return SqlDbType.Time;
            else if (type == typeof(Guid))
                return SqlDbType.UniqueIdentifier;
            else if (type == typeof(bool))
                return SqlDbType.Bit;
            else if (type == typeof(byte))
                return SqlDbType.Binary;
            else if (type == typeof(SqlMoney))
                return SqlDbType.Money;
            else if (type == typeof(SqlXml))
                return SqlDbType.Xml;
            else if (type == typeof(SqlGuid))
                return SqlDbType.UniqueIdentifier;
            else if (type == typeof(SqlBinary))
                return SqlDbType.VarBinary;
            else if (type == typeof(SqlBoolean))
                return SqlDbType.Bit;
            else if (type == typeof(SqlByte))
                return SqlDbType.TinyInt;
            else if (type == typeof(SqlDateTime))
                return SqlDbType.DateTime;
            else if (type == typeof(SqlDecimal))
                return SqlDbType.Decimal;
            else if (type == typeof(SqlDouble))
                return SqlDbType.Float;
            else if (type == typeof(SqlSingle))
                return SqlDbType.Real;
            else if (type == typeof(SqlInt16))
                return SqlDbType.SmallInt;
            else if (type == typeof(SqlInt32))
                return SqlDbType.Int;
            else if (type == typeof(SqlInt64))
                return SqlDbType.BigInt;
            else if (type == typeof(SqlString))
                return SqlDbType.NVarChar;
            else
                throw new ArgumentException("Unsupported type: " + type.FullName);
        }

        virtual public bool ExecuteQuery(IDbConnection connection)
        {
            try
            {
                IDbCommand command = connection.CreateCommand();
                command.CommandTimeout = 30;

                string headers = GetColumnHeadersForQuery();
                if (string.IsNullOrEmpty(headers))
                    return false;

                string request = GetQueryText(headers);
                command.CommandText = request;
                using (IDataReader reader = command.ExecuteReader())
                {
                    SqlDbType[] types = new SqlDbType[reader.FieldCount];
                    if (types.Length != _columns.Count)
                    {
                        //Logger.LogInformation($"Number of retrieved columns does not match number of queried columns for table {Table}.");
                        return false;
                    }

                    for (int i = 0; i < types.Length; i++)
                    {
                        types[i] = GetSqlDbTypeFromType(reader.GetFieldType(i));
                    }

                    int count = 0;
                    while (reader.Read())
                    {
                        count++;
                        for (int i = 0; i < _columns.Count; i++)
                        {
                            switch (types[i])
                            {
                                case SqlDbType.VarChar:
                                case SqlDbType.NVarChar:
                                case SqlDbType.Text:
                                case SqlDbType.NChar:
                                case SqlDbType.Char:
                                    (_columns[i] as TDFDataColumn<string>).GetValueList().Add(reader.GetString(i));
                                    break;
                                case SqlDbType.Int:
                                case SqlDbType.BigInt:
                                case SqlDbType.TinyInt:
                                case SqlDbType.SmallInt:
                                    (_columns[i] as TDFDataColumn<long>).GetValueList().Add(reader.GetInt64(i));
                                    break;
                                case SqlDbType.Float:
                                case SqlDbType.Real:
                                case SqlDbType.Decimal:
                                    (_columns[i] as TDFDataColumn<double>).GetValueList().Add(reader.GetDouble(i));
                                    break;
                                default:
                                    //Logger.LogInformation($"Unsupported type loaded in {Table} {i} {types[i]}");
                                    break;
                            }                          
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        protected string GetQueryText(string columnHeadersForQuery)
        {
            return $"SELECT {columnHeadersForQuery} FROM {_tableName}";
        }

        public void Print()
        {
            //Logger.LogInformation($"Printing {Table}\t{Columns.Count} * {KeyList.Count} entries.");
        }

        public override bool Equals(Object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            if (obj == null || GetType() != obj.GetType())
                return false;
            TDFDataTable that = obj as TDFDataTable;
            return _tableName.Equals(that._tableName) &&
                   _keyColumnName.Equals(that._keyColumnName) &&
                   _columns.SequenceEqual(that._columns) &&
                   _keyColumn.Values.SequenceEqual(that._keyColumn.Values);
        }
    }
}

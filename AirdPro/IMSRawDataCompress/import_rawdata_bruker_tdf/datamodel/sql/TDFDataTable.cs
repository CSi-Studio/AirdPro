using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;

namespace AirdPro.IMSRawDataCompress.import_rawdata_bruker_tdf.datamodel.sql
{
    public abstract class TDFDataTable
    {
        protected  string tableName;
        protected  string keyColumnName;
        protected  List<IDataColumn<Object>> columns;
        protected IDataColumn<Object> keyColumn;

        public String GetTableName() { return tableName; }
        //public String GetEntryHeader() { return keyColumnName; }
        public String GetKeyColumnName() { return keyColumnName; }
        public TDFDataTable(string tableName)
        {
            this.tableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            this.columns = new List<IDataColumn<Object>>();
        }

        public List<IDataColumn<Object>> GetColumns()
        {
            return columns;
        }

        public IDataColumn<Object> GetKeyColumn()
        {
            return keyColumn;
        }

        public void AddKeyColumn(IDataColumn<Object> column)
        {
            if (column == null)
                throw new ArgumentNullException(nameof(column));

            //Attention: put KeyColumn to first position of List!!!
            columns.Insert(0, column);
            this.keyColumn = column;
            this.keyColumnName = column.ColumnName;
        }

        public void AddColumn(IDataColumn<Object> column)
        {
            if (column == null)
                throw new ArgumentNullException(nameof(column));

            //put Column to last position of List
            columns.Add(column);
        }

        public IDataColumn<Object> GetColumn(string columnName)
        {
            foreach (var column in columns)
            {
                if (column.ColumnName.Equals(columnName))
                    return column as TDFDataColumn<Object>;
            }
            return null;
        }

        public virtual string GetColumnNamesForQuery()
        {
            var headers = new StringBuilder();
            foreach (var col in columns)
            {
                headers.Append(col.ColumnName + ", ");
            }
            if (headers.Length > 0)
                headers.Remove(headers.Length - 2, 2);

            return headers.ToString();
        }

        public bool IsValid()
        {
            long numKeys = keyColumn.Values.Count();
            foreach (var col in columns)
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

        public virtual bool ExecuteQuery(IDbConnection connection)
        {
            try
            {
                IDbCommand command = connection.CreateCommand();
                command.CommandTimeout = 30;

                string headers = GetColumnNamesForQuery();
                if (string.IsNullOrEmpty(headers))
                    return false;

                string request = GetQueryText(headers);
                command.CommandText = request;
                using (IDataReader reader = command.ExecuteReader())
                {
                    SqlDbType[] types = new SqlDbType[reader.FieldCount];
                    if (types.Length != columns.Count)
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
                        for (int i = 0; i < columns.Count; i++)
                        {
                            switch (types[i])
                            {
                                case SqlDbType.VarChar:
                                case SqlDbType.NVarChar:
                                case SqlDbType.Text:
                                case SqlDbType.NChar:
                                case SqlDbType.Char:
                                    (columns[i] as TDFDataColumn<string>).GetValueList().Add(reader.GetString(i));
                                    break;
                                case SqlDbType.Int:
                                case SqlDbType.BigInt:
                                case SqlDbType.TinyInt:
                                case SqlDbType.SmallInt:
                                    (columns[i] as TDFDataColumn<long>).GetValueList().Add(reader.GetInt64(i));
                                    break;
                                case SqlDbType.Float:
                                case SqlDbType.Real:
                                case SqlDbType.Decimal:
                                    (columns[i] as TDFDataColumn<double>).GetValueList().Add(reader.GetDouble(i));
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

        public virtual string GetQueryText(string columnHeadersForQuery)
        {
            return $"SELECT {columnHeadersForQuery} FROM {tableName}";
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
            return tableName.Equals(that.tableName) &&
                   keyColumnName.Equals(that.keyColumnName) &&
                   columns.SequenceEqual(that.columns) &&
                   keyColumn.Values.SequenceEqual(that.keyColumn.Values);
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}

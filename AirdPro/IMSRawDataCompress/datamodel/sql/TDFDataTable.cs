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
    public abstract class TDFDataTable<Object>
    {
        protected readonly string Table;
        protected readonly string EntryHeader;
        protected readonly List<TDFDataColumn<Object>> Columns;
        protected readonly TDFDataColumn<Object> KeyList;

        public String getTable() { return Table; }
        public String getEntryHeader() { return EntryHeader; }
        public TDFDataTable(string table, string entryHeader)
        {
            this.Table = table ?? throw new ArgumentNullException(nameof(table));
            this.EntryHeader = entryHeader ?? throw new ArgumentNullException(nameof(entryHeader));
            Columns = new List<TDFDataColumn<Object>>();
            KeyList = new TDFDataColumn<Object>(entryHeader);
            Columns.Add(KeyList);
        }

        public void AddColumn(TDFDataColumn<Object> column)
        {
            if (column == null)
                throw new ArgumentNullException(nameof(column));

            Columns.Add(column);
        }

        public TDFDataColumn<Object> GetColumn(string columnName)
        {
            foreach (var column in Columns)
            {
                if (column.GetColumnName() == columnName)
                    return column;
            }
            return null;
        }

        protected string GetColumnHeadersForQuery()
        {
            var headers = new StringBuilder();
            foreach (var col in Columns)
            {
                headers.Append(col.GetColumnName() + ", ");
            }
            if (headers.Length > 0)
                headers.Remove(headers.Length - 2, 2);

            return headers.ToString();
        }

        public bool IsValid()
        {
            long numKeys = KeyList.Count;
            foreach (var col in Columns)
            {
                if (numKeys != col.Count)
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
                    if (types.Length != Columns.Count)
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
                        Console.WriteLine("第" + count + "行:");
                        for (int i = 0; i < Columns.Count; i++)
                        {
                            switch (types[i])
                            {
                                case SqlDbType.VarChar:
                                case SqlDbType.NVarChar:
                                case SqlDbType.Text:
                                case SqlDbType.NChar:
                                case SqlDbType.Char:
                                    (Columns[i] as TDFDataColumn<String>).Add(reader.GetString(i)); 
                                    break;
                                case SqlDbType.Int:
                                case SqlDbType.BigInt:
                                case SqlDbType.TinyInt:
                                case SqlDbType.SmallInt:
                                    (Columns[i] as TDFDataColumn<long>).Add(reader.GetInt64(i));
                                    break;
                                case SqlDbType.Float:
                                case SqlDbType.Real:
                                case SqlDbType.Decimal:
                                    (Columns[i] as TDFDataColumn<double>).Add(reader.GetDouble(i));
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
            return $"SELECT {columnHeadersForQuery} FROM {Table}";
        }

        public void Print()
        {
            //Logger.LogInformation($"Printing {Table}\t{Columns.Count} * {KeyList.Count} entries.");
        }

        public List<TDFDataColumn<Object>> getColumns()
        {
            return Columns;
        }

        public bool Equals(Object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            if (obj == null || GetType() != obj.GetType())
                return false;
            TDFDataTable<Object> that = obj as TDFDataTable<Object>;
            return Table == that.Table &&
                   EntryHeader == that.EntryHeader &&
                   Columns.SequenceEqual(that.Columns) &&
                   KeyList.SequenceEqual(that.KeyList);
        }
    }
}

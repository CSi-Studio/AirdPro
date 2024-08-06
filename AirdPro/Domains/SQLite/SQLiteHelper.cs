using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.IO;

namespace AirdPro.Domains.Sqlite
{
    internal class SQLiteHelper
    {
        // 用于与SQLite数据库交互的连接对象
        private SQLiteConnection connection;
        // 操作的表名
        private string tableName;
        // 表的列名，以逗号分隔的字符串
        private string columnNameStr;
        //表的列名
        private string[] columnNames;

        /// <summary>
        /// 通过指定的数据库文件路径初始化SQLiteManager类的实例。
        /// </summary>
        /// <param name="dbAddress">数据库文件的路径。</param>
        public SQLiteHelper(string dbAddress)
        {
            // 创建SQLite连接字符串构建器，并设置数据源和版本
            var connectionStringBuilder = new SQLiteConnectionStringBuilder
            {
                DataSource = dbAddress,
                Version = 3
            };

            // 通过连接字符串构建器创建SQLite连接对象
            connection = new SQLiteConnection(connectionStringBuilder.ConnectionString);
            // 打开数据库连接
            connection.Open();
        }

        /// <summary>
        /// 关闭数据库连接。
        /// </summary>
        public void Close()
        {
            // 如果连接不为空且状态为打开，则关闭连接
            if (connection != null && connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        /// <summary>
        /// 创建表，包括指定的列和类型。
        /// </summary>
        /// <param name="tableName">要创建的表名。</param>
        /// <param name="hasAutoIncrementId">是否自动添加自增ID。</param>
        /// <param name="columns">列名数组。</param>
        /// <param name="columnTypes">列类型数组。</param>
        public void CreateTable(string tableName, bool hasAutoIncrementId, string[] columns, Type[] columnTypes)
        {
            // 设置当前操作的表名
            this.tableName = tableName;
            // 设置列名字符串
            columnNameStr = string.Join(",", columns);
            columnNames = columns;

            // 创建列定义列表
            var columnDefinitions = new List<string>();
            // 如果需要自动添加ID列
            if (hasAutoIncrementId)
            {
                columnDefinitions.Add("ID INTEGER PRIMARY KEY AUTOINCREMENT");
            }
            // 遍历列类型数组，添加列定义
            for (int i = 0; i < columns.Length; i++)
            {
                var columnName = columns[i];
                var columnTypeStr = GetColumnType(columnTypes[i]);
                columnDefinitions.Add($"{columnName} {columnTypeStr}");
            }

            // 构建列定义字符串
            string columnDefinitionsStr = string.Join(", ", columnDefinitions);
            // 构建创建表的SQL语句
            string sqlStr = $"CREATE TABLE IF NOT EXISTS {tableName} ({columnDefinitionsStr});";
            // 执行非查询SQL命令创建表
            ExecuteNonQuery(sqlStr);
        }

        /// <summary>
        /// 删除当前的表
        /// </summary>
        public void DeleteTable()
        {
            string sql = $"DROP TABLE IF EXISTS {tableName};";
            ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 创建索引以提高查询效率，在创建表之后使用
        /// </summary>
        /// <param name="columnName">要创建索引的列名。</param>
        public void CreateIndex(string columnName)
        {
            string sql = $"CREATE INDEX IF NOT EXISTS {columnName} ON {tableName} ({columnName});";
            ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 销毁指定的索引。
        /// </summary>
        /// <param name="indexName">要删除的索引的名称。</param>
        public void DeleteIndex(string columnName)
        {
            string sql = $"DROP INDEX IF EXISTS {columnName};";
            ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 获取C#类型对应的SQLite类型字符串。
        /// </summary>
        /// <param name="type">C#中的类型。</param>
        /// <returns>对应的SQLite类型字符串。</returns>
        private string GetColumnType(Type type)
        {
            // 根据C#类型返回对应的SQLite类型字符串
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    return "INTEGER";
                case TypeCode.Double:
                    return "REAL";
                case TypeCode.Single:
                    return "FLOAT";
                case TypeCode.DateTime:
                    return "DATETIME";
                case TypeCode.Boolean:
                    return "BOOLEAN";

                default:
                    return "TEXT";
            }
        }

        /// <summary>
        /// 向表中插入记录。
        /// </summary>
        /// <param name="values">要插入的值的数组。</param>
        /// <returns>插入操作影响的行数。</returns>
        public int Insert(params object[] values)
        {
            // 创建参数列表并初始化
            var parameters = values.Select((value, index) => new SQLiteParameter($"@{index}", value)).ToArray();
            // 构建参数化SQL语句
            var parameterNames = string.Join(", ", parameters.Select(p => p.ParameterName));
            // 构建插入数据的SQL语句
            string sql = $"INSERT INTO {tableName} ({columnNameStr}) VALUES ({parameterNames});";
            // 执行非查询SQL命令并返回影响的行数
            return ExecuteNonQuery(sql, parameters);
        }

        /// <summary>
        /// 获取多条件的字符串组合
        /// </summary>
        /// <param name="bAnd">True为And逻辑，False 为 OR 逻辑</param>
        /// <param name="condition1"></param>
        /// <param name="condition2"></param>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public string GetMultiContidion(bool bAnd, string condition1, string condition2, params string[] conditions)
        {
            if (bAnd)
            {
                if (conditions != null && conditions.Length > 0)
                {
                    string str1 = string.Join(" And ", conditions);
                    return string.Join(" And ", condition1, condition2, str1);
                }
                else
                {
                    return string.Join(" And ", condition1, condition2);
                }


            }
            else
            {
                if (conditions != null && conditions.Length > 0)
                {
                    string str1 = string.Join(" OR ", conditions);
                    return string.Join(" OR ", condition1, condition2, str1);
                }
                else
                {
                    return string.Join(" OR ", condition1, condition2);
                }
            }
        }

        /// <summary>
        /// 根据条件删除记录。
        /// </summary>
        /// <param name="condition">删除条件。</param>
        /// <returns>删除操作影响的行数。</returns>
        public int Delete(string condition)
        {

            // 构建删除数据的SQL语句
            string sql = $"DELETE FROM {tableName} WHERE {condition};";

            // 执行非查询SQL命令并返回影响的行数
            return ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 更新表中的记录。
        /// </summary>
        /// <param name="columnName">要更新的列名。</param>
        /// <param name="value">新的值。</param>
        /// <param name="condition">更新条件。</param>
        /// <returns>更新操作影响的行数。</returns>
        public int Update(string columnName, object value, string condition)
        {
            // 构建更新数据的SQL语句
            string query = $"UPDATE {tableName} SET {columnName} = @{value} WHERE {condition};";
            // 创建参数对象并添加到SQL命令中
            var parameter = new SQLiteParameter(value.ToString(), value);
            // 执行非查询SQL命令并返回影响的行数
            return ExecuteNonQuery(query, parameter);
        }

        /// <summary>
        /// 根据条件查询列的值。
        /// </summary>
        /// <param name="columnName">要查询的列名。</param>
        /// <param name="condition">查询条件。</param>
        /// <returns>查询结果的值。</returns>
        public object GetValue(string columnName, string condition)
        {

            // 构建查询数据的SQL语句
            string selectQuery = $"SELECT {columnName} FROM {tableName} WHERE {condition};";
            // 执行查询SQL命令并返回查询结果
            return ExecuteScalar(selectQuery);
        }

        /// <summary>
        /// 根据条件查询列的值。
        /// </summary>
        /// <param name="columnName">要查询的列名。</param>
        /// <param name="condition">查询条件。</param>
        /// <returns>查询结果的值。</returns>
        public List<object> GetValues(string columnName, string condition)
        {
            List<object> values = new List<object>();

            string selectQuery = "";

            if (string.IsNullOrWhiteSpace(condition))
            {
                selectQuery = $"SELECT {columnName} FROM {tableName};";
            }
            else
            {
                selectQuery = $"SELECT {columnName} FROM {tableName} WHERE {condition};";
            }

            try
            {
                using (var reader = ExecuteQuery(selectQuery))
                {
                    while (reader.Read())
                    {
                        values.Add(reader[columnName]);
                    }
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
            }


            return values;
        }

        /// <summary>
        /// 根据条件获取所有行的数据
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public List<Dictionary<string, object>> GetRowDatas(string condition)
        {
            List<Dictionary<string, object>> values = new List<Dictionary<string, object>>();

            string selectQuery = "";

            if (string.IsNullOrWhiteSpace(condition))
            {
                selectQuery = $"SELECT {columnNameStr} FROM {tableName};";
            }
            else
            {
                selectQuery = $"SELECT {columnNameStr} FROM {tableName} WHERE {condition};";
            }

            try
            {
                using (var reader = ExecuteQuery(selectQuery))
                {
                    while (reader.Read())
                    {
                        Dictionary<string, object> dict = new Dictionary<string, object>();
                        foreach (var columnName in columnNames)
                        {
                            dict.Add(columnName, reader[columnName]);
                        }
                        values.Add(dict);
                    }
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
            }


            return values;
        }



        /// <summary>
        /// 执行非查询SQL命令（如INSERT, UPDATE, DELETE）。
        /// </summary>
        /// <param name="sql">SQL命令字符串。</param>
        /// <param name="parameters">SQL命令参数数组。</param>
        /// <returns>命令执行影响的行数。</returns>
        public int ExecuteNonQuery(string sql, params SQLiteParameter[] parameters)
        {
            try
            {
                // 使用SQLiteCommand对象执行SQL命令
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    return command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // 记录异常信息到日志文件
                LogException(ex);
                return 0;
            }
        }

        /// <summary>
        /// 执行查询SQL命令（如SELECT），返回SQLiteDataReader对象。
        /// </summary>
        /// <param name="sql">SQL命令字符串。</param>
        /// <returns>SQLiteDataReader对象。</returns>
        private SQLiteDataReader ExecuteQuery(string sql)
        {
            try
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    return command.ExecuteReader();
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                return null;
            }
        }

        /// <summary>
        /// 执行查询SQL命令（如SELECT），返回单个结果。
        /// </summary>
        /// <param name="sql">SQL命令字符串。</param>
        /// <returns>查询结果的单个值。</returns>
        private object ExecuteScalar(string sql)
        {
            try
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    return command.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                return null;
            }
        }

        /// <summary>
        /// 记录异常信息到日志文件。
        /// </summary>
        /// <param name="ex">要记录的异常对象。</param>
        private void LogException(Exception ex)
        {
            // 将异常信息追加到日志文件中
            string errorMessage = $"发生错误：{ex.Message}{Environment.NewLine}{ex.StackTrace}";
            File.AppendAllText("error.log", errorMessage);
        }
    }
}

using System;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayer
{
    public abstract class DataAccess : IDataAccess
    {
        private readonly string _connectionString;

        protected DataAccess(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void ExecuteNonQuery(string sqlText, SqlParameter[] parameters, CommandType type = CommandType.Text)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                using (SqlCommand sql = connection.CreateCommand())
                {
                    sql.CommandType = type;
                    sql.CommandText = sqlText;
                    if (parameters != null)
                    {
                        sql.Parameters.AddRange(parameters);
                    }

                    sql.ExecuteNonQuery();
                }
            }
        }

        public T ExecuteScalar<T>(string sqlText, SqlParameter[] parameters, CommandType type = CommandType.Text)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                using (SqlCommand sql = connection.CreateCommand())
                {
                    sql.CommandType = type;
                    sql.CommandText = sqlText;
                    if (parameters != null)
                    {
                        sql.Parameters.AddRange(parameters);
                    }

                    return GetNULLSafe<T>(sql.ExecuteScalar());
                }
            }
        }

        public DataTable GetData(string sqlText, SqlParameter[] parameters, CommandType type = CommandType.Text)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                using (SqlCommand sql = connection.CreateCommand())
                {
                    sql.CommandType = type;
                    sql.CommandText = sqlText;
                    if (parameters != null)
                    {
                        sql.Parameters.AddRange(parameters);
                    }

                    using (SqlDataReader reader = sql.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        DataTable table = new DataTable();
                        table.Load(reader);
                        return table;
                    }
                }
            }
        }

        /// <summary>
        /// Used for performing actions over a data set
        /// </summary>
        /// <example> 
        /// This sample shows how to call the <see cref="GetData"/> method.
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main() 
        ///     {
        ///         string sql = "SELECT firstColumn, secondColumn FROM LotsOData WHERE firstColumn > @param1";
        ///         var parameters = new[] { new SqlParameter("@param1", SqlDbType.Int) { Value = 100 } };
        ///
        ///       _dal.GetData(sql, parameters,
        ///         reader =>
        ///             {
        ///                   while (reader.Read())
        ///                   {
        ///                       do stuff
        ///                   }
        ///             });
        ///     }
        /// }
        /// </code>
        /// </example>
        public void GetData(string sqlText, SqlParameter[] parameters, Action<IDataReader> action, CommandType type = CommandType.Text)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                using (SqlCommand sql = connection.CreateCommand())
                {
                    sql.CommandType = type;
                    sql.CommandText = sqlText;
                    if (parameters != null)
                    {
                        sql.Parameters.AddRange(parameters);
                    }

                    using (SqlDataReader reader = sql.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        action(reader);
                    }
                }
            }
        }

        /// <summary>
        /// Used for SQL queries with paging
        /// </summary>
        /// <example> 
        /// This sample shows how to call the <see cref="GenerateSqlPagedDataString"/> method.
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main() 
        ///     {
        ///         string sql = _dal.GenerateSqlPagedDataString("firstColumn, secondColumn", "firstColumn", "SELECT firstColumn, secondColumn FROM LotsOData");
        ///         var parameters = new[] { new SqlParameter("@PageNum", SqlDbType.Int) { Value = 1 }, new SqlParameter("@PageSize", SqlDbType.Int) { Value = 10 } };
        ///
        ///       _dal.GetData(sql, parameters,
        ///         reader =>
        ///             {
        ///                   while (reader.Read())
        ///                   {
        ///                       do stuff
        ///                   }
        ///             });
        ///     }
        /// }
        /// </code>
        /// </example>
        public string GenerateSqlPagedDataString(string select, string orderBy, string sql)
        {
            return string.Format("SELECT {0} FROM ( SELECT *, ROW_NUMBER() OVER(ORDER BY {1}) as RowNum FROM ( {2} ) AS temp ) as DerivedTableName WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize", select, orderBy, sql);
        }

        private static T GetNULLSafe<T>(object val)
        {
            return val == null || val == DBNull.Value
                ? default(T)
                : (T)val;
        }
    }
}

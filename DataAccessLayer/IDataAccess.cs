using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayer
{
    public interface IDataAccess
    {
        void ExecuteNonQuery(string sqlText, SqlParameter[] parameters, CommandType type = CommandType.Text);
        T ExecuteScalar<T>(string sqlText, SqlParameter[] parameters, CommandType type = CommandType.Text);
        DataTable GetData(string sqlText, SqlParameter[] parameters, CommandType type = CommandType.Text);
        string GenerateSqlPagedDataString(string select, string orderBy, string sql);
    }
}

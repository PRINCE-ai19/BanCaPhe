using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace BanCaPhe.Models
{
    internal class DoUongDbConnection
    {
        private static readonly string _connectionString = ConfigurationManager
                                .ConnectionStrings["DefaultConnection"].ConnectionString;

        public static IDbConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}

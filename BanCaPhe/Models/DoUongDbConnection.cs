using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanCaPhe.Models
{
    internal class DoUongDbConnection
    {
        public static SqlConnection GetConnection()
        {
            string connStr = ConfigurationManager
                            .ConnectionStrings["DefaultConnection"].ConnectionString;

            return new SqlConnection(connStr);
        }
    }
}

using BanCaPhe.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanCaPhe.Services
{
    public class KichThuocService
    {

        public List<KichThuoc> GetBySanPhamId(int sanPhamId)
        {
            var list = new List<KichThuoc>();

            using (SqlConnection conn = DoUongDbConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetKichThuocBySanPham", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SanPhamID", sanPhamId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new KichThuoc
                    {
                        ID = (int)reader["ID"],
                        SanPhamID = (int)reader["SanPhamID"],
                        TenKichThuoc = reader["TenKichThuoc"].ToString(),
                        DungTich = reader["DungTich"].ToString(),
                        Gia = (decimal)reader["Gia"]
                    });
                }
            }

            return list;
        }
    }
}

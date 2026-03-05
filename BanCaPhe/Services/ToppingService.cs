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
    internal class ToppingService
    {
        public List<Topping> GetAll()
        {
            var list = new List<Topping>();

            using (SqlConnection conn = DoUongDbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_Topping_GetAll", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();

                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(new Topping
                        {
                            ID = Convert.ToInt32(rd["ID"]),
                            TenTopping = rd["TenTopping"].ToString(),
                            Gia = Convert.ToDecimal(rd["Gia"]),
                            CoTheBanRieng = Convert.ToBoolean(rd["CoTheBanRieng"]),
                            ConBan = Convert.ToBoolean(rd["ConBan"]),
                            HinhAnh = rd["HinhAnh"]?.ToString()
                        });
                    }
                }
            }

            return list;
        }

        public Topping GetById(int id)
        {
            Topping topping = null;

            using (SqlConnection conn = DoUongDbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_Topping_GetById", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", id);

                conn.Open();
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    if (rd.Read())
                    {
                        topping = new Topping
                        {
                            ID = Convert.ToInt32(rd["ID"]),
                            TenTopping = rd["TenTopping"].ToString(),
                            Gia = Convert.ToDecimal(rd["Gia"]),
                            CoTheBanRieng = Convert.ToBoolean(rd["CoTheBanRieng"]),
                            ConBan = Convert.ToBoolean(rd["ConBan"]),
                            HinhAnh = rd["HinhAnh"]?.ToString()
                        };
                    }
                }
            }

            return topping;
        }

        public bool Insert(Topping t)
        {
            using (SqlConnection conn = DoUongDbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_Topping_Insert", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TenTopping", t.TenTopping);
                cmd.Parameters.AddWithValue("@Gia", t.Gia);
                cmd.Parameters.AddWithValue("@CoTheBanRieng", t.CoTheBanRieng);
                cmd.Parameters.AddWithValue("@ConBan", t.ConBan);
                cmd.Parameters.AddWithValue("@HinhAnh", (object)t.HinhAnh ?? DBNull.Value);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Update(Topping t)
        {
            using (SqlConnection conn = DoUongDbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_Topping_Update", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID", t.ID);
                cmd.Parameters.AddWithValue("@TenTopping", t.TenTopping);
                cmd.Parameters.AddWithValue("@Gia", t.Gia);
                cmd.Parameters.AddWithValue("@CoTheBanRieng", t.CoTheBanRieng);
                cmd.Parameters.AddWithValue("@ConBan", t.ConBan);
                cmd.Parameters.AddWithValue("@HinhAnh", (object)t.HinhAnh ?? DBNull.Value);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }


        public bool Delete(int id)
        {
            using (SqlConnection conn = DoUongDbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_Topping_Delete", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", id);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Disable(int id)
        {
            using (SqlConnection conn = DoUongDbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_Topping_Disable", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", id);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}

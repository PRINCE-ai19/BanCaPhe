using BanCaPhe.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanCaPhe.Services
{
    internal class DoUongService
    {
        public List<DoUong> GetAll()
        {
            var list = new List<DoUong>();

            using (SqlConnection conn = DoUongDbConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DoUong_GetAll", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new DoUong
                    {
                        ID = (int)reader["ID"],
                        TenDoUong = reader["TenDoUong"].ToString(),
                        Gia = Convert.ToDecimal(reader["Gia"]),
                        LoaiID = (int)reader["LoaiID"],  
                        TenLoai = reader["TenLoai"].ToString(),
                        Mota = reader["Mota"].ToString(),
                        ConBan = (bool)reader["ConBan"],
                        HinhAnh = reader["HinhAnh"].ToString()
                    });
                }
            }

            return list;

        }


        public bool CheckTen(string tenDoUong, int loaiId, int? id = null)
        {
            using (SqlConnection conn = DoUongDbConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DoUong_CheckTen", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TenDoUong", tenDoUong);
                cmd.Parameters.AddWithValue("@LoaiID", loaiId);
                cmd.Parameters.AddWithValue("@ID", (object?)id ?? DBNull.Value);

                conn.Open();
                int count = (int)cmd.ExecuteScalar();

                return count > 0;
            }
        }

        public bool Insert(DoUong d)
        {
            using (SqlConnection conn = DoUongDbConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DoUong_Insert", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TenDoUong", d.TenDoUong);
                cmd.Parameters.AddWithValue("@Gia", d.Gia);
                cmd.Parameters.AddWithValue("@LoaiID", d.LoaiID);
                cmd.Parameters.AddWithValue("@Mota", d.Mota);
                cmd.Parameters.AddWithValue("@ConBan", d.ConBan);
                cmd.Parameters.AddWithValue("@HinhAnh", d.HinhAnh);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Update(DoUong d)
        {
            using (SqlConnection conn = DoUongDbConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DoUong_Update", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID", d.ID);
                cmd.Parameters.AddWithValue("@TenDoUong", d.TenDoUong);
                cmd.Parameters.AddWithValue("@Gia", d.Gia);
                cmd.Parameters.AddWithValue("@LoaiID", d.LoaiID);
                cmd.Parameters.AddWithValue("@Mota", d.Mota);
                cmd.Parameters.AddWithValue("@ConBan", d.ConBan);
                cmd.Parameters.AddWithValue("@HinhAnh", d.HinhAnh);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Delete(int id)
        {
            using (SqlConnection conn = DoUongDbConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DoUong_Delete", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID", id);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public List<DanhMucDouong> GetLoai()
        {
            List<DanhMucDouong> list = new List<DanhMucDouong>();

            using (SqlConnection conn = DoUongDbConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT ID, TenLoai FROM LoaiDoUong", conn);
                conn.Open();

                var rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    list.Add(new DanhMucDouong
                    {
                        ID = (int)rd["ID"],
                        TenLoai = rd["TenLoai"].ToString()
                    });
                }
            }
            return list;
        }

        public ObservableCollection<DoUong> GetAllNV()
        {
            var list = new ObservableCollection<DoUong>();

            using (SqlConnection conn = DoUongDbConnection.GetConnection())
            {
                conn.Open();

                string sql = "SELECT ID, TenDoUong, Gia, LoaiID, HinhAnh FROM DoUong";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    list.Add(new DoUong
                    {
                        ID = (int)rd["ID"],
                        TenDoUong = rd["TenDoUong"].ToString(),
                        Gia = (decimal)rd["Gia"],
                        LoaiID = (int)rd["LoaiID"],
                        HinhAnh = rd["HinhAnh"]?.ToString()
                    });
                }
            }

            return list;
        }

    }
}

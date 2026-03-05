using BanCaPhe.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace BanCaPhe.Services
{
    internal class LoaiDoUongService
    {
        public List<DanhMucDouong> GetAll()
        {
            var list = new List<DanhMucDouong>();

            using (SqlConnection conn = DoUongDbConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_LoaiDoUong_GetAll", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new DanhMucDouong
                    {
                        ID = (int)reader["ID"],
                        TenLoai = reader["TenLoai"].ToString(),
                        ViTri = (int)reader["ViTri"]
                    });
                }
            }

            return list;
        }


        public bool Insert(DanhMucDouong loai)
        {
            using (SqlConnection conn = DoUongDbConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_LoaiDoUong_Insert", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TenLoai", loai.TenLoai);
                cmd.Parameters.AddWithValue("@ViTri", loai.ViTri);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }


        public bool Update(DanhMucDouong loai)
        {
            using (SqlConnection conn = DoUongDbConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_LoaiDoUong_Update", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID", loai.ID);
                cmd.Parameters.AddWithValue("@TenLoai", loai.TenLoai);
                cmd.Parameters.AddWithValue("@ViTri", loai.ViTri);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Delete(int id)
        {
            using (SqlConnection conn = DoUongDbConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_LoaiDoUong_Delete", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID", id);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool IsTenLoaiExists(string tenLoai , int ViTri)
        {
            using (SqlConnection conn = DoUongDbConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_LoaiDoUong_CheckTenLoai", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TenLoai", tenLoai);
                cmd.Parameters.AddWithValue("@ViTri", ViTri);

                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }

        }


        public ObservableCollection<DanhMucDouong> GetAllND()
        {
            var list = new ObservableCollection<DanhMucDouong>();

            using (SqlConnection conn = DoUongDbConnection.GetConnection())
            {
                conn.Open();

                string sql = "SELECT ID, TenLoai, ViTri FROM LoaiDoUong ORDER BY ViTri";

                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    list.Add(new DanhMucDouong
                    {
                        ID = (int)rd["ID"],
                        TenLoai = rd["TenLoai"].ToString(),
                        ViTri = (int)rd["ViTri"]
                    });
                }
            }

            return list;
        }

    }
}

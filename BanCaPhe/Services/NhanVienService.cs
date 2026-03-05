using BanCaPhe.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanCaPhe.Services
{

    internal class NhanVienService
    {

        public void DangKy(NhanVien nv)
        {
            using (SqlConnection conn = DoUongDbConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_NhanVien_DangKy", conn);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HoTen", nv.HoTen);
                cmd.Parameters.AddWithValue("@Email", nv.Email);
                cmd.Parameters.AddWithValue("@MatKhau", nv.MatKhau);
                cmd.Parameters.AddWithValue("@SoDienThoai", nv.SoDienThoai);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }



        public NhanVien DangNhap(DangNhap nv)
        {
            using (SqlConnection conn = DoUongDbConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_NhanVien_DangNhap", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TaiKhoan", nv.TaiKhoan);
                cmd.Parameters.AddWithValue("@MatKhau", nv.MatKhau);

                conn.Open();

                using var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new NhanVien
                    {
                        ID = (int)reader["Id"],
                        HoTen = reader["HoTen"].ToString(),
                        Email = reader["Email"].ToString(),
                        VaiTro = reader["VaiTro"].ToString()
                    };
                }

                return null;
            }

        }


        public List<NhanVien> GetAll()
        {
            var list = new List<NhanVien>();

            using (SqlConnection conn = DoUongDbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_NhanVien_GetAll", conn))
            { 
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();

                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(new NhanVien {

                            ID = Convert.ToInt32(rd["ID"]),
                            HoTen = rd["HoTen"].ToString(),
                            Email = rd["Email"].ToString(),
                            SoDienThoai = rd["SoDienThoai"].ToString(),
                            VaiTro = rd["VaiTro"].ToString(),
                            TrangThai = Convert.ToBoolean(rd["TrangThai"]),
                            HinhAnh = rd["HinhAnh"]?.ToString()
                        });
                    }
                }
            }
            return list;
        }


        public NhanVien GetById(int id)
        {
            using (SqlConnection conn = DoUongDbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_NhanVien_GetById", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", id);
                conn.Open();

                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    if (rd.Read())
                    {
                        return new NhanVien
                        {
                            ID = Convert.ToInt32(rd["ID"]),
                            HoTen = rd["HoTen"]?.ToString(),
                            Email = rd["Email"]?.ToString(),
                            SoDienThoai = rd["SoDienThoai"]?.ToString(),
                            MatKhau = rd["MatKhau"]?.ToString(),  
                            VaiTro = rd["VaiTro"]?.ToString(),
                            TrangThai = Convert.ToBoolean(rd["TrangThai"]),
                            HinhAnh = rd["HinhAnh"]?.ToString()
                        };
                    }
                }
            }
            return null;
        }

        public bool Insert(NhanVien nv)
        {
            using (SqlConnection conn = DoUongDbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_NhanVien_Insert", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@HoTen", nv.HoTen);
                cmd.Parameters.AddWithValue("@MatKhau", nv.MatKhau);
                cmd.Parameters.AddWithValue("@Email", nv.Email);
                cmd.Parameters.AddWithValue("@SoDienThoai", nv.SoDienThoai);
                cmd.Parameters.AddWithValue("@VaiTro", nv.VaiTro);
                cmd.Parameters.AddWithValue("@TrangThai", nv.TrangThai);
                cmd.Parameters.AddWithValue("@HinhAnh", (object)nv.HinhAnh ?? DBNull.Value);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Update(NhanVien nv)
        {
            using (SqlConnection conn = DoUongDbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_NhanVien_Update", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID", nv.ID);
                cmd.Parameters.AddWithValue("@HoTen", nv.HoTen);
                cmd.Parameters.AddWithValue("@Email", nv.Email);
                cmd.Parameters.AddWithValue("@SoDienThoai", nv.SoDienThoai);
                cmd.Parameters.AddWithValue("@VaiTro", nv.VaiTro);
                cmd.Parameters.AddWithValue("@TrangThai", nv.TrangThai);
                cmd.Parameters.AddWithValue("@HinhAnh", (object)nv.HinhAnh ?? DBNull.Value);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool UpdatePassword(int id, string newHashedPassword)
        {
            using (SqlConnection conn = DoUongDbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_NhanVien_UpdatePassword", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@MatKhau", newHashedPassword);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }


        public bool Delete(int id)
        {
            using (SqlConnection conn = DoUongDbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_NhanVien_Delete", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", id);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}

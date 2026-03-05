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
    internal class DoanhThuService
    {
        public DoanhThuThangModel GetDoanhThuThangNay()
        {
            DoanhThuThangModel result = null;

            using (SqlConnection conn = DoUongDbConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SP_GetDoanhThuThangNay", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    result = new DoanhThuThangModel
                    {
                        TongDoanhThu = reader["TongDoanhThu"] != DBNull.Value
                            ? Convert.ToDecimal(reader["TongDoanhThu"])
                            : 0,
                        TongDonHang = reader["TongDonHang"] != DBNull.Value
                            ? (int)reader["TongDonHang"]
                            : 0,
                        Thang = reader["Thang"] != DBNull.Value
                            ? (int)reader["Thang"]
                            : 0,
                        Nam = reader["Nam"] != DBNull.Value
                            ? (int)reader["Nam"]
                            : 0
                    };
                }
            }

            return result ?? new DoanhThuThangModel();
        }

        public ThongKeDonHangModel GetTongDonHangThangNay()
        {
            ThongKeDonHangModel result = null;

            using (SqlConnection conn = DoUongDbConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SP_GetTongDonHangThangNay", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    result = new ThongKeDonHangModel
                    {
                        TongSoDonHang = reader["TongSoDonHang"] != DBNull.Value
                            ? (int)reader["TongSoDonHang"]
                            : 0,
                        DonHangCoDoanhThu = reader["DonHangCoDoanhThu"] != DBNull.Value
                            ? (int)reader["DonHangCoDoanhThu"]
                            : 0,
                        DoanhThuTrungBinh = reader["DoanhThuTrungBinh"] != DBNull.Value
                            ? Convert.ToDecimal(reader["DoanhThuTrungBinh"])
                            : 0,
                        DonHangLonNhat = reader["DonHangLonNhat"] != DBNull.Value
                            ? Convert.ToDecimal(reader["DonHangLonNhat"])
                            : 0,
                        DonHangNhoNhat = reader["DonHangNhoNhat"] != DBNull.Value
                            ? Convert.ToDecimal(reader["DonHangNhoNhat"])
                            : 0
                    };
                }
            }

            return result ?? new ThongKeDonHangModel();
        }

    
        public List<SanPhamBanChayModel> GetSanPhamBanChayNhat(int topN = 10)
        {
            var list = new List<SanPhamBanChayModel>();

            using (SqlConnection conn = DoUongDbConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SP_GetSanPhamBanChayNhat", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TopN", topN);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new SanPhamBanChayModel
                    {
                        SanPhamID = reader["SanPhamID"] != DBNull.Value
                            ? (int)reader["SanPhamID"]
                            : 0,
                        TenKichThuoc = reader["TenKichThuoc"]?.ToString() ?? "",
                        DungTich = reader["DungTich"]?.ToString() ?? "",
                        TongSoLuongBan = reader["TongSoLuongBan"] != DBNull.Value
                            ? (int)reader["TongSoLuongBan"]
                            : 0,
                        TongDoanhThu = reader["TongDoanhThu"] != DBNull.Value
                            ? Convert.ToDecimal(reader["TongDoanhThu"])
                            : 0,
                        SoDonHang = reader["SoDonHang"] != DBNull.Value
                            ? (int)reader["SoDonHang"]
                            : 0,
                        GiaTrungBinh = reader["GiaTrungBinh"] != DBNull.Value
                            ? Convert.ToDecimal(reader["GiaTrungBinh"])
                            : 0
                    });
                }
            }

            return list;
        }

        
        public List<DoanhThuTheoNgayModel> GetChiTietDoanhThuTheoNgay(int? thang = null, int? nam = null)
        {
            var list = new List<DoanhThuTheoNgayModel>();

            using (SqlConnection conn = DoUongDbConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SP_GetChiTietDoanhThuTheoNgay", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Thang", (object?)thang ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Nam", (object?)nam ?? DBNull.Value);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new DoanhThuTheoNgayModel
                    {
                        Ngay = reader["Ngay"] != DBNull.Value
                            ? Convert.ToDateTime(reader["Ngay"])
                            : DateTime.Now,
                        SoDonHang = reader["SoDonHang"] != DBNull.Value
                            ? (int)reader["SoDonHang"]
                            : 0,
                        DoanhThuSanPham = reader["DoanhThuSanPham"] != DBNull.Value
                            ? Convert.ToDecimal(reader["DoanhThuSanPham"])
                            : 0,
                        DoanhThuTopping = reader["DoanhThuTopping"] != DBNull.Value
                            ? Convert.ToDecimal(reader["DoanhThuTopping"])
                            : 0,
                        TongDoanhThu = reader["TongDoanhThu"] != DBNull.Value
                            ? Convert.ToDecimal(reader["TongDoanhThu"])
                            : 0
                    });
                }
            }

            return list;
        }

        public List<ToppingPhoBienModel> GetToppingPhoBienNhat(int topN = 5)
        {
            var list = new List<ToppingPhoBienModel>();

            using (SqlConnection conn = DoUongDbConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SP_GetToppingPhoBienNhat", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TopN", topN);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new ToppingPhoBienModel
                    {
                        ID = reader["ID"] != DBNull.Value
                            ? (int)reader["ID"]
                            : 0,
                        TenTopping = reader["TenTopping"]?.ToString() ?? "",
                        GiaTopping = reader["GiaTopping"] != DBNull.Value
                            ? Convert.ToDecimal(reader["GiaTopping"])
                            : 0,
                        TongSoLuong = reader["TongSoLuong"] != DBNull.Value
                            ? (int)reader["TongSoLuong"]
                            : 0,
                        TongDoanhThu = reader["TongDoanhThu"] != DBNull.Value
                            ? Convert.ToDecimal(reader["TongDoanhThu"])
                            : 0,
                        SoLanDuocOrder = reader["SoLanDuocOrder"] != DBNull.Value
                            ? (int)reader["SoLanDuocOrder"]
                            : 0
                    });
                }
            }

            return list;
        }


        public DoanhThuKhoangThoiGianModel GetDoanhThuTheoKhoangThoiGian(DateTime tuNgay, DateTime denNgay)
        {
            DoanhThuKhoangThoiGianModel result = null;

            using (SqlConnection conn = DoUongDbConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SP_GetDoanhThuTheoKhoangThoiGian", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TuNgay", tuNgay);
                cmd.Parameters.AddWithValue("@DenNgay", denNgay);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    result = new DoanhThuKhoangThoiGianModel
                    {
                        TuNgay = tuNgay,
                        DenNgay = denNgay,
                        TongDonHang = reader["TongDonHang"] != DBNull.Value
                            ? (int)reader["TongDonHang"]
                            : 0,
                        DoanhThuSanPham = reader["DoanhThuSanPham"] != DBNull.Value
                            ? Convert.ToDecimal(reader["DoanhThuSanPham"])
                            : 0,
                        DoanhThuTopping = reader["DoanhThuTopping"] != DBNull.Value
                            ? Convert.ToDecimal(reader["DoanhThuTopping"])
                            : 0,
                        TongDoanhThu = reader["TongDoanhThu"] != DBNull.Value
                            ? Convert.ToDecimal(reader["TongDoanhThu"])
                            : 0,
                        DoanhThuTrungBinhMoiDon = reader["DoanhThuTrungBinhMoiDon"] != DBNull.Value
                            ? Convert.ToDecimal(reader["DoanhThuTrungBinhMoiDon"])
                            : 0
                    };
                }
            }

            return result ?? new DoanhThuKhoangThoiGianModel
            {
                TuNgay = tuNgay,
                DenNgay = denNgay
            };
        }
    }
}

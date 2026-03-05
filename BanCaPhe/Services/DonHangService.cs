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
    public class DonHangService
    {
        public void ThanhToan(DonHang donHang, List<OrderItem> items)
        {
            using SqlConnection conn = DoUongDbConnection.GetConnection();
            using SqlCommand cmd = new SqlCommand("sp_ThanhToan", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@NgayLap", donHang.NgayLap);
            cmd.Parameters.AddWithValue("@NhanVienID", donHang.NhanVienID);
            cmd.Parameters.AddWithValue("@TongTien", donHang.TongTien);
            cmd.Parameters.AddWithValue("@HinhThucThanhToan", donHang.HinhThucThanhToan);

            // CHI TIẾT ĐƠN HÀNG 
            DataTable tbChiTiet = new DataTable();
            tbChiTiet.Columns.Add("SanPhamKichThuocID", typeof(int));
            tbChiTiet.Columns.Add("SoLuong", typeof(int));
            tbChiTiet.Columns.Add("Gia", typeof(decimal));

            foreach (var item in items)
            {
                tbChiTiet.Rows.Add(
                    item.SanPhamKichThuocID,
                    item.SoLuong,
                    item.DonGia
                );
            }

            var p1 = cmd.Parameters.AddWithValue("@ChiTietDonHang", tbChiTiet);
            p1.SqlDbType = SqlDbType.Structured;
            p1.TypeName = "dbo.TVP_ChiTietDonHang";

            // CHI TIẾT TOPPING 
            DataTable tbTopping = new DataTable();
            tbTopping.Columns.Add("ChiTietDonHangIndex", typeof(int));
            tbTopping.Columns.Add("ToppingID", typeof(int));
            tbTopping.Columns.Add("SoLuong", typeof(int));
            tbTopping.Columns.Add("Gia", typeof(decimal));

            for (int i = 0; i < items.Count; i++)
            {
                foreach (var tp in items[i].Toppings)
                {
                    tbTopping.Rows.Add(
                        i + 1,
                        tp.ToppingID,
                        tp.SoLuong,
                        tp.Gia
                    );
                }
            }

            var p2 = cmd.Parameters.AddWithValue("@ChiTietTopping", tbTopping);
            p2.SqlDbType = SqlDbType.Structured;
            p2.TypeName = "dbo.TVP_ChiTietTopping";

            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}

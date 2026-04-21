using BanCaPhe.Helpers;
using BanCaPhe.Models;
using Dapper;
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

            var parameters = new DynamicParameters();
            parameters.Add("@NgayLap", donHang.NgayLap);
            parameters.Add("@NhanVienID", donHang.NhanVienID);
            parameters.Add("@TongTien", donHang.TongTien);
            parameters.Add("@HinhThucThanhToan", donHang.HinhThucThanhToan);
            parameters.Add("@KhachHangID", donHang.KhachHangID);
            parameters.Add("@DungVoucher", donHang.DungVoucher);
                
       
            parameters.Add("@ChiTietDonHang", tbChiTiet.AsTableValuedParameter("dbo.TVP_ChiTietDonHang"));
            parameters.Add("@ChiTietTopping", tbTopping.AsTableValuedParameter("dbo.TVP_ChiTietTopping"));

   
            StoreHelper.Execute("sp_ThanhToan", parameters);
        }
    }
}

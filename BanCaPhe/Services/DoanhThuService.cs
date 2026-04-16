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
    internal class DoanhThuService
    {
        public DoanhThuThangModel GetDoanhThuThangNay()
        {
            return StoreHelper.QueryFirstOrDefault<DoanhThuThangModel>("SP_GetDoanhThuThangNay") ?? new DoanhThuThangModel();
        }

        public ThongKeDonHangModel GetTongDonHangThangNay()
        {
            return StoreHelper.QueryFirstOrDefault<ThongKeDonHangModel>("SP_GetTongDonHangThangNay") ?? new ThongKeDonHangModel();
        }

        public List<SanPhamBanChayModel> GetSanPhamBanChayNhat(int topN = 10)
        {
            return StoreHelper.QueryList<SanPhamBanChayModel>("SP_GetSanPhamBanChayNhat", new { TopN = topN });
        }

        public List<DoanhThuTheoNgayModel> GetChiTietDoanhThuTheoNgay(int? thang = null, int? nam = null)
        {
            return StoreHelper.QueryList<DoanhThuTheoNgayModel>("SP_GetChiTietDoanhThuTheoNgay", new { Thang = thang, Nam = nam });
        }

        public List<ToppingPhoBienModel> GetToppingPhoBienNhat(int topN = 5)
        {
            return StoreHelper.QueryList<ToppingPhoBienModel>("SP_GetToppingPhoBienNhat", new { TopN = topN });
        }

        public DoanhThuKhoangThoiGianModel GetDoanhThuTheoKhoangThoiGian(DateTime tuNgay, DateTime denNgay)
        {
            var result = StoreHelper.QueryFirstOrDefault<DoanhThuKhoangThoiGianModel>(
                "SP_GetDoanhThuTheoKhoangThoiGian",
                new { TuNgay = tuNgay, DenNgay = denNgay });

            if (result != null)
            {
                result.TuNgay = tuNgay;
                result.DenNgay = denNgay;
            }

            return result ?? new DoanhThuKhoangThoiGianModel { TuNgay = tuNgay, DenNgay = denNgay };
        }
    }
}

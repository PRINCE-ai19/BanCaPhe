using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanCaPhe.Models
{
    public class DoanhThuKhoangThoiGianModel
    {
        public DateTime TuNgay { get; set; }
        public DateTime DenNgay { get; set; }
        public int TongDonHang { get; set; }
        public decimal DoanhThuSanPham { get; set; }
        public decimal DoanhThuTopping { get; set; }
        public decimal TongDoanhThu { get; set; }
        public decimal DoanhThuTrungBinhMoiDon { get; set; }

        public string KhoangThoiGianDisplay => $"{TuNgay:dd/MM/yyyy} - {DenNgay:dd/MM/yyyy}";
        public string TongDoanhThuDisplay => $"{TongDoanhThu:N0} đ";
        public string DoanhThuSanPhamDisplay => $"{DoanhThuSanPham:N0} đ";
        public string DoanhThuToppingDisplay => $"{DoanhThuTopping:N0} đ";
        public string DoanhThuTrungBinhDisplay => $"{DoanhThuTrungBinhMoiDon:N0} đ";
    }
}

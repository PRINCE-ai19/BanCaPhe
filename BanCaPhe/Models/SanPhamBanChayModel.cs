using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanCaPhe.Models
{
    public class SanPhamBanChayModel
    {
        public int SanPhamID { get; set; }
        public string TenKichThuoc { get; set; }
        public string DungTich { get; set; }
        public int TongSoLuongBan { get; set; }
        public decimal TongDoanhThu { get; set; }
        public int SoDonHang { get; set; }
        public decimal GiaTrungBinh { get; set; }

        public string TenHienThi => $"{TenKichThuoc} - {DungTich}";
        public string DoanhThuDisplay => $"{TongDoanhThu:N0} đ";
        public string GiaTrungBinhDisplay => $"{GiaTrungBinh:N0} đ";
    }
}

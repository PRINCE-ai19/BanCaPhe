using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanCaPhe.Models
{
    public class ThongKeDonHangModel
    {
        public int TongSoDonHang { get; set; }
        public int DonHangCoDoanhThu { get; set; }
        public decimal DoanhThuTrungBinh { get; set; }
        public decimal DonHangLonNhat { get; set; }
        public decimal DonHangNhoNhat { get; set; }

        public string DoanhThuTrungBinhDisplay => $"{DoanhThuTrungBinh:N0} đ";
        public string DonHangLonNhatDisplay => $"{DonHangLonNhat:N0} đ";
        public string DonHangNhoNhatDisplay => $"{DonHangNhoNhat:N0} đ";
    }
}

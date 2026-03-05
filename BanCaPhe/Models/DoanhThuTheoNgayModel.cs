using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanCaPhe.Models
{
    public class DoanhThuTheoNgayModel
    {
        public DateTime Ngay { get; set; }
        public int SoDonHang { get; set; }
        public decimal DoanhThuSanPham { get; set; }
        public decimal DoanhThuTopping { get; set; }
        public decimal TongDoanhThu { get; set; }

        public string NgayDisplay => Ngay.ToString("dd/MM/yyyy");
        public string DoanhThuDisplay => $"{TongDoanhThu:N0} đ";
    }
}

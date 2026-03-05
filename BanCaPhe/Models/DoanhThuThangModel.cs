using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanCaPhe.Models
{
    public class DoanhThuThangModel
    {
        public decimal TongDoanhThu { get; set; }
        public int TongDonHang { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; }

        public string ThangNamDisplay => $"Tháng {Thang}/{Nam}";
        public string DoanhThuDisplay => $"{TongDoanhThu:N0} đ";
    }
}

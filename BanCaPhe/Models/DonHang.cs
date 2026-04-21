using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanCaPhe.Models
{
    public class DonHang
    {
        public DateTime NgayLap { get; set; }
        public int NhanVienID { get; set; }
        public decimal TongTien { get; set; }
        public string HinhThucThanhToan { get; set; }
        public int? KhachHangID { get; set; }
        public bool DungVoucher { get; set; }
    }
}

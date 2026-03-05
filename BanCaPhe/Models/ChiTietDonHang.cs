using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanCaPhe.Models
{
    public class ChiTietDonHang
    {
        public int ID { get; set; }
        public int DonHangID { get; set; }
        public int SanPhamKichThuocID { get; set; }
        public int SoLuong { get; set; }
        public decimal Gia { get; set; }
    }
}

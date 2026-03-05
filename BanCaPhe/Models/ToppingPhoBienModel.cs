using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanCaPhe.Models
{
    public class ToppingPhoBienModel
    {
        public int ID { get; set; }
        public string TenTopping { get; set; }
        public decimal GiaTopping { get; set; }
        public int TongSoLuong { get; set; }
        public decimal TongDoanhThu { get; set; }
        public int SoLanDuocOrder { get; set; }

        public string DoanhThuDisplay => $"{TongDoanhThu:N0} đ";
        public string GiaToppingDisplay => $"{GiaTopping:N0} đ";
    }
}

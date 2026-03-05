using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanCaPhe.Models
{
    public class ChiTietTopping
    {
        public int ID { get; set; }
        public int ChiTietDonHangID { get; set; }
        public int ToppingID { get; set; }
        public int SoLuong { get; set; }
        public decimal Gia { get; set; }
    }
}

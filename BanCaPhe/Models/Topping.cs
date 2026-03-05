using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanCaPhe.Models
{
    public class Topping
    {
        public int ID { get; set; }
        public string TenTopping { get; set; }
        public decimal Gia { get; set; }
        public bool CoTheBanRieng { get; set; }
        public bool ConBan { get; set; }
        public string HinhAnh { get; set; }


    }
}

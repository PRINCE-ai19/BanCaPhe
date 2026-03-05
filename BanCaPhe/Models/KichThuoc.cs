using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanCaPhe.Models
{
   public class KichThuoc 
    {
        public int ID { get; set; }
        public int SanPhamID { get; set; }
        public string TenKichThuoc { get; set; } 
        public string DungTich { get; set; }     
        public decimal Gia { get; set; }

        public bool IsSelected { get; set; }
    }
}

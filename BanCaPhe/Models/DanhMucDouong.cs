using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanCaPhe.Models
{

    public enum KieuHienThi
    {
        DoUong,
        Topping
    }
    public class DanhMucDouong
    {
        public int ID { get; set; }         
        public string TenLoai { get; set; }
        public int ViTri { get; set; }
    }
}

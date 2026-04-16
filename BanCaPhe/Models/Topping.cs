using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanCaPhe.Models
{
    public class Topping
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Tên Topping không được để trống")]
        [StringLength(100, ErrorMessage = "Tên Topping tối đa 100 ký tự")]
        public string TenTopping { get; set; }

        [Required(ErrorMessage = "Giá Topping không được để trống")]
        [Range(0, 1000000, ErrorMessage = "Giá Topping phải từ 0 trở lên")]
        public decimal Gia { get; set; }

        public bool CoTheBanRieng { get; set; }
        public bool ConBan { get; set; }

        [StringLength(255)]
        public string? HinhAnh { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanCaPhe.Models
{
    public class KichThuoc
    {
        public int ID { get; set; }
        public int SanPhamID { get; set; }

        [Required(ErrorMessage = "Tên kích thước không được để trống")]
        [StringLength(50)]
        public string TenKichThuoc { get; set; }

        [StringLength(50)]
        public string? DungTich { get; set; }

        [Required(ErrorMessage = "Giá kích thước không được để trống")]
        [Range(0, 10000000, ErrorMessage = "Giá phải từ 0 trở lên")]
        public decimal Gia { get; set; }

        public bool IsSelected { get; set; }
    }
}

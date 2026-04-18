using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanCaPhe.Models
{
    public class DoUong
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Tên đồ uống không được để trống")]
        [StringLength(100, ErrorMessage = "Tên đồ uống tối đa 100 ký tự")]
        public string TenDoUong { get; set; }

        [Required(ErrorMessage = "Giá không được để trống")]
        [Range(0, 1000000000, ErrorMessage = "Giá phải lớn hơn hoặc bằng 0")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Gia { get; set; }

        public bool ConBan { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn loại đồ uống")]
        public int? LoaiID { get; set; }

        public string? TenLoai { get; set; }

        [StringLength(50, ErrorMessage = "Mô tả tối đa 500 ký tự")]
        public string? Mota { get; set; }

        public string? HinhAnh { get; set; }
    }
}

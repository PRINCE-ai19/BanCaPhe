using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanCaPhe.Models
{
    public class DanhMucDouong
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Tên loại không được để trống")]
        [StringLength(100, ErrorMessage = "Tên loại tối đa 100 ký tự")]
        public string TenLoai { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập vị trí hiển thị")]
        [Range(1, 1000, ErrorMessage = "Vị trí phải nằm trong khoảng từ 1 đến 1000")]
        public int ViTri { get; set; }
    }
}

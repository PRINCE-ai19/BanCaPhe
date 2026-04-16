using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanCaPhe.Models
{
    public class NhanVien
    {
        public int ID { get; set; }
        [StringLength(100)]
        public string HoTen { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Định dạng email không hợp lệ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6 đến 50 ký tự")]
        public string MatKhau { get; set; }

        [RegularExpression(@"^(0|\+84)(\d{9,10})$", ErrorMessage = "Số điện thoại không hợp lệ (phải có 10 hoặc 11 số)")]
        public string SoDienThoai { get; set; }
        public string VaiTro { get; set; }
        public bool TrangThai { get; set; }

        public string HinhAnh { get; set; }
    }
}

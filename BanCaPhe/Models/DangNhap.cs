using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanCaPhe.Models
{
    internal class DangNhap
    {
       public string HoTen { get; set; }

        [Required(ErrorMessage = "Email hoặc số điện thoại không được để trống")]
        [StringLength(100, ErrorMessage = "Tối đa 100 ký tự")]
        public string TaiKhoan { get; set; }


        [Required(ErrorMessage = "Mật khẩu không được để trống")]
   
        public string MatKhau { get; set; }
    }
}

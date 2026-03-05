using BanCaPhe.Helpers;
using BanCaPhe.Models;
using BanCaPhe.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace BanCaPhe
{
   
    public partial class DangKy : Window
    {
        public DangKy()
        {
            InitializeComponent();
 
        }

        private void BtnDangKy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var nhanVien = new NhanVien
                {
                    HoTen = txtHoTen.Text,
                    Email = txtEmail.Text,
                    MatKhau = txtMatKhau.Password, 
                    SoDienThoai = txtSoDienThoai.Text,
                    VaiTro = "Employee"
                };

                var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                bool isValid = Validator.TryValidateObject(
                    nhanVien,
                    new ValidationContext(nhanVien),
                    results,
                    true
                );

                if (!isValid)
                {
                    MessageBox.Show(
                        string.Join("\n", results.Select(x => x.ErrorMessage)),
                        "Lỗi dữ liệu"
                    );
                    return;
                }

                var service = new NhanVienService();

                nhanVien.MatKhau = PasswordHelper.HashPassword(nhanVien.MatKhau);

                service.DangKy(nhanVien);

                MessageBox.Show("Đăng ký thành công");
                W_DangNhap dangNhap = new W_DangNhap();
                dangNhap.Show();
                this.Close();

            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    MessageBox.Show("Email hoặc số điện thoại đã tồn tại");
                }
                else
                {
                    MessageBox.Show("Lỗi hệ thống");
                }
            }

         
        }

        private void TextDangNhap_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            W_DangNhap dangNhap = new W_DangNhap();
            dangNhap.Show();
            this.Close();
        }
    }
}

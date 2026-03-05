using BanCaPhe.Helpers;
using BanCaPhe.Models;
using BanCaPhe.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
   
    public partial class W_DangNhap : Window
    {
        public W_DangNhap()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
          
        }

        private void BtnDangNhap_Click(object sender, RoutedEventArgs e)
        {

            var model = new DangNhap
            {
                TaiKhoan = txtEmail.Text,
                MatKhau = PasswordHelper.HashPassword(txtMatKhau.Password)
            };

            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            bool isValid = Validator.TryValidateObject(
                model,
                new ValidationContext(model),
                results,
                true
            );

            if (!isValid)
            {
                MessageBox.Show(string.Join("\n", results.Select(x => x.ErrorMessage)));
                return;
            }

            var service = new NhanVienService();
            var nhanVien = service.DangNhap(model);

            if (nhanVien == null)
            {
                MessageBox.Show("Sai tài khoản hoặc mật khẩu");
                return;
            }

           
            UserSession.CurrentUser = nhanVien;

            if(nhanVien.VaiTro == "Admin")
            {
                MessageBox.Show("Đăng nhập thành công, xin chào Chủ Nhân");
                AdminDashboard admin = new AdminDashboard();
                admin.Show();
                this.Close();
            }
            else if (nhanVien.VaiTro == "Employee")
            {
                MessageBox.Show($"Đăng nhập thành công, xin chào {nhanVien.HoTen}");

                MainWindow main = new MainWindow();
                Application.Current.MainWindow = main;
                main.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Mày là ai");
                return;
            }

          
        }

        private void TextDagKy_Click(object sender, RoutedEventArgs e)
        {
            DangKy dangKy = new DangKy();
            dangKy.Show();
            this.Close();
        }
        private void TextQuyenmatkhau_Click(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Chuyển sang màn hình khác (đăng ký / quên mật khẩu)");
        }
    }
}
  
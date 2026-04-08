using BanCaPhe.Helpers;
using BanCaPhe.Models;
using BanCaPhe.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace BanCaPhe
{
    public partial class W_DangNhap : Window
    {
        public W_DangNhap()
        {
            InitializeComponent();
        }

        #region [ HIỆU ỨNG CHUYỂN MÀN HÌNH ]

        private void SwitchToRegister(object sender, MouseButtonEventArgs e)
        {
            // Bật Panel đăng ký (nhưng Opacity = 0)
            pnlRegister.Visibility = Visibility.Visible;
            
            // Khởi tạo Animation
            var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.2));
            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.2)) { BeginTime = TimeSpan.FromSeconds(0.2) };

            // Sau khi mờ xong Panel Login thì tắt hẳn nó đi
            fadeOut.Completed += (s, ev) => pnlLogin.Visibility = Visibility.Collapsed;

            // Xóa hết form
            ClearRegisterForm();

            // Chạy Animation
            pnlLogin.BeginAnimation(UIElement.OpacityProperty, fadeOut);
            pnlRegister.BeginAnimation(UIElement.OpacityProperty, fadeIn);
        }

        private void SwitchToLogin(object sender, MouseButtonEventArgs e)
        {
            // Bật Panel đăng nhập (Opacity = 0)
            pnlLogin.Visibility = Visibility.Visible;
            
            // Khởi tạo Animation
            var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.2));
            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.2)) { BeginTime = TimeSpan.FromSeconds(0.2) };

            // Tắt hẳn Panel Register khi fadeOut xong
            fadeOut.Completed += (s, ev) => pnlRegister.Visibility = Visibility.Collapsed;

            // Xóa form
            txtEmail.Clear();
            txtMatKhau.Clear();

            // Chạy Animation
            pnlRegister.BeginAnimation(UIElement.OpacityProperty, fadeOut);
            pnlLogin.BeginAnimation(UIElement.OpacityProperty, fadeIn);
        }

        private void ClearRegisterForm()
        {
            txtHoTen.Clear();
            txtRegEmail.Clear();
            txtRegMatKhau.Clear();
            txtXacNhan.Clear();
            txtSoDienThoai.Clear();
        }

        #endregion


        #region [ LOGIC ĐĂNG NHẬP ]

        private void BtnDangNhap_Click(object sender, RoutedEventArgs e)
        {
            var model = new DangNhap
            {
                TaiKhoan = txtEmail.Text.Trim(),
                MatKhau = PasswordHelper.HashPassword(txtMatKhau.Password)
            };

            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(
                model,
                new ValidationContext(model),
                results,
                true
            );

            if (!isValid)
            {
                MessageBox.Show(string.Join("\n", results.Select(x => x.ErrorMessage)), "Lỗi dữ liệu");
                return;
            }

            var service = new NhanVienService();
            var nhanVien = service.DangNhap(model);

            if (nhanVien == null)
            {
                MessageBox.Show("Sai tài khoản hoặc mật khẩu", "Thất bại");
                return;
            }

            UserSession.CurrentUser = nhanVien;

            if (nhanVien.VaiTro == "Admin")
            {
                MessageBox.Show("Đăng nhập thành công, xin chào Quản Trị Viên");
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
                MessageBox.Show("Không thể ủy quyền tài khoản này!");
            }
        }

        private void TextQuyenmatkhau_Click(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Chức năng khôi phục mật khẩu đang được nâng cấp.");
        }

        #endregion


        #region [ LOGIC ĐĂNG KÝ ]

        private void BtnDangKy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtRegMatKhau.Password != txtXacNhan.Password)
                {
                    MessageBox.Show("Mật khẩu xác nhận không trùng khớp!", "Lỗi xác nhận");
                    return;
                }

                var nhanVien = new NhanVien
                {
                    HoTen = txtHoTen.Text.Trim(),
                    Email = txtRegEmail.Text.Trim(),
                    MatKhau = txtRegMatKhau.Password,
                    SoDienThoai = txtSoDienThoai.Text.Trim(),
                    VaiTro = "Employee"
                };

                var results = new List<ValidationResult>();
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

                // Mã hóa password giống y như BE cũ
                nhanVien.MatKhau = PasswordHelper.HashPassword(nhanVien.MatKhau);

                service.DangKy(nhanVien);

                MessageBox.Show("Đăng ký thành công! Hãy đăng nhập hệ thống.", "Hoàn tất");
                
                // Tự động chuyển qua tab Login
                SwitchToLogin(null, null);
                
                // Điền sẵn giúp Email vào tab đăng nhập
                txtEmail.Text = nhanVien.Email;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    MessageBox.Show("Email hoặc số điện thoại này đã được sử dụng!", "Lỗi dữ liệu");
                }
                else
                {
                    MessageBox.Show("Lỗi hệ thống: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
            }
        }

        #endregion
    }
}
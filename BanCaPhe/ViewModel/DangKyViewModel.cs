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
using System.Windows.Input;

namespace BanCaPhe.ViewModel
{
    public class DangKyViewModel : BaseViewModel
    {
        private string _hoTen;
        public string HoTen
        {
            get => _hoTen;
            set { _hoTen = value; OnPropertyChanged(); }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        private string _soDienThoai;
        public string SoDienThoai
        {
            get => _soDienThoai;
            set { _soDienThoai = value; OnPropertyChanged(); }
        }

        public ICommand DangKyCommand { get; }
        public ICommand QuayLaiCommand { get; }

        public DangKyViewModel()
        {
            DangKyCommand = new RelayCommand(ExecuteDangKy);
            QuayLaiCommand = new RelayCommand(ExecuteQuayLai);
        }

        private void ExecuteDangKy(object obj)
        {
            // Lấy mật khẩu từ PasswordBox (vì PasswordBox không hỗ trợ Binding trực tiếp để bảo mật)
            var passwordBox = obj as System.Windows.Controls.PasswordBox;
            string rawPassword = passwordBox?.Password;

            if (string.IsNullOrWhiteSpace(rawPassword))
            {
                DialogService.ShowError("Vui lòng nhập mật khẩu");
                return;
            }

            var nhanVien = new NhanVien
            {
                HoTen = HoTen?.Trim(),
                Email = Email?.Trim(),
                MatKhau = rawPassword,
                SoDienThoai = SoDienThoai?.Trim(),
                VaiTro = "Employee"
            };

            if (!ValidationHelper.ValidateWithReport(nhanVien)) return;

            try
            {
                var service = new NhanVienService();
              
                nhanVien.MatKhau = PasswordHelper.HashPassword(nhanVien.MatKhau);

              
                service.DangKy(nhanVien);

                DialogService.ShowMessage("Đăng ký thành công! Vui lòng đăng nhập.");
                WindowService.ShowLoginWindow();
                CloseCurrentWindow();
            }
            catch (Exception ex)
            {
                DialogService.ShowError(ex.Message);
            }
        }

        private void ExecuteQuayLai(object obj)
        {
            WindowService.ShowLoginWindow();
            CloseCurrentWindow();
        }

        private void CloseCurrentWindow()
        {
            var currentWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.DataContext == this);
            currentWindow?.Close();
        }
    }
}

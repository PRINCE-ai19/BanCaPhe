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
    public class DangNhapViewModel : BaseViewModel
    {
        private string _email;
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        private string _matKhau;
        public string MatKhau
        {
            get => _matKhau;
            set { _matKhau = value; OnPropertyChanged(); }
        }

        public ICommand DangNhapCommand { get; }
        public ICommand MoDangKyCommand { get; }

        public DangNhapViewModel()
        {
            DangNhapCommand = new RelayCommand(ExecuteDangNhap);
            MoDangKyCommand = new RelayCommand(ExecuteMoDangKy);
        }

        private void ExecuteDangNhap(object obj)
        {
            var passwordBox = obj as System.Windows.Controls.PasswordBox;
            string rawPassword = passwordBox?.Password;

            var model = new DangNhap
            {
                TaiKhoan = Email,
                MatKhau = !string.IsNullOrEmpty(rawPassword) ? PasswordHelper.HashPassword(rawPassword) : ""
            };

        
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(model, new ValidationContext(model), results, true);

            if (!isValid)
            {
                DialogService.ShowError(string.Join("\n", results.Select(x => x.ErrorMessage)));
                return;
            }

            var service = new NhanVienService();
            var nhanVien = service.DangNhap(model);

            if (nhanVien == null)
            {
                DialogService.ShowError("Sai tài khoản hoặc mật khẩu");
                return;
            }

            UserSession.CurrentUser = nhanVien;

            if (nhanVien.VaiTro == "Admin")
            {
                DialogService.ShowMessage("Đăng nhập thành công, xin chào Chủ Nhân");
                WindowService.ShowAdminDashboard();
            }
            else
            {
                DialogService.ShowMessage($"Đăng nhập thành công, xin chào {nhanVien.HoTen}");
                WindowService.ShowMainWindow();
            }

            var currentWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.DataContext == this);
            currentWindow?.Close();
        }

        private void ExecuteMoDangKy(object obj)
        {
            WindowService.ShowRegisterWindow();
            var currentWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.DataContext == this);
            currentWindow?.Close();
        }
    }
}

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
using System.Windows.Input;

namespace BanCaPhe.ViewModel
{
    public class DoiMatKhauViewModel : BaseViewModel
    {
        private readonly NhanVienService _service = new NhanVienService();
        private int _nhanVienId;

        private string _hoTen;
        public string HoTen
        {
            get => _hoTen;
            set { _hoTen = value; OnPropertyChanged(); }
        }

        public ICommand DoiMatKhauCommand { get; }
        public ICommand HuyCommand { get; }

        public DoiMatKhauViewModel(int nhanVienId, string hoTen)
        {
            _nhanVienId = nhanVienId;
            HoTen = hoTen;

            DoiMatKhauCommand = new RelayCommand(ExecuteDoiMatKhau);
            HuyCommand = new RelayCommand(ExecuteHuy);
        }

        private void ExecuteDoiMatKhau(object obj)
        {
            var values = obj as object[];
            if (values == null || values.Length < 2) return;

            var pbMoi = values[0] as PasswordBox;
            var pbXacNhan = values[1] as PasswordBox;

            var request = new DoiMatKhauRequest
            {
                MatKhauMoi = pbMoi?.Password,
                XacNhanMatKhau = pbXacNhan?.Password
            };

       
            if (!ValidationHelper.ValidateWithReport(request)) return;

            try
            {
       
                string hashedPassword = PasswordHelper.HashPassword(request.MatKhauMoi);
                bool result = _service.UpdatePassword(_nhanVienId, hashedPassword);

                if (result)
                {
                    DialogService.ShowMessage("Đổi mật khẩu thành công!");
                    CloseWindow();
                }
                else
                {
                    DialogService.ShowError("Đổi mật khẩu thất bại!");
                }
            }
            catch (Exception ex)
            {
           
                DialogService.ShowError(ex.Message);
            }
        }

        private void ExecuteHuy(object obj)
        {
            CloseWindow();
        }

        private void CloseWindow()
        {
            var window = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.DataContext == this);
            if (window != null)
            {
                window.DialogResult = true;
                window.Close();
            }
        }
    }

  
}

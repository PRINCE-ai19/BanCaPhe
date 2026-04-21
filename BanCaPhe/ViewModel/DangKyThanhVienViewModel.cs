using BanCaPhe.Helpers;
using BanCaPhe.Models;
using BanCaPhe.Services;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BanCaPhe.ViewModel
{
    public class DangKyThanhVienViewModel : BaseViewModel
    {
        private string _hoTen;
        public string HoTen
        {
            get => _hoTen;
            set { _hoTen = value; OnPropertyChanged(); }
        }

        private string _soDienThoai;
        public string SoDienThoai
        {
            get => _soDienThoai;
            set { _soDienThoai = value; OnPropertyChanged(); }
        }

        public ICommand DangKyCommand { get; }
        public ICommand HuyCommand { get; }

        public DangKyThanhVienViewModel()
        {
            DangKyCommand = new RelayCommand(_ => ExecuteDangKy());
            HuyCommand = new RelayCommand(_ => CloseWindow());
        }

        private void ExecuteDangKy()
        {
            var khachHang = new KhachHang
            {
                HoTen = HoTen?.Trim(),
                SoDienThoai = SoDienThoai?.Trim()
            };

            if (!ValidationHelper.ValidateWithReport(khachHang)) return;

            try
            {
                var service = new KhachHangService();
                var result = service.Register(khachHang.HoTen, khachHang.SoDienThoai);

                if (result != null)
                {
                    DialogService.ShowMessage("Đăng ký thành viên thành công! Khách hàng nhận được mức giảm giá 2%.");
                    CloseWindow();
                }
            }
            catch (Exception ex)
            {
                DialogService.ShowError("Lỗi đăng ký: " + ex.Message);
            }
        }

        private void CloseWindow()
        {
            Application.Current.Windows.OfType<Window>()
                .FirstOrDefault(w => w.DataContext == this)
                ?.Close();
        }
    }
}

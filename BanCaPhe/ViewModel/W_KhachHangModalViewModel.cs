using BanCaPhe.Helpers;
using BanCaPhe.Models;
using BanCaPhe.Services;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BanCaPhe.ViewModel
{
    public class W_KhachHangModalViewModel : BaseViewModel
    {
        private readonly KhachHangService _khachHangService;
        private int _id;
        private bool _isEdit;

        public string Title => _isEdit ? "CHỈNH SỬA KHÁCH HÀNG" : "THÊM KHÁCH HÀNG MỚI";

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

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public W_KhachHangModalViewModel()
        {
            _khachHangService = new KhachHangService();
            _isEdit = false;
            SaveCommand = new RelayCommand(_ => ExecuteSave());
            CancelCommand = new RelayCommand(_ => CloseWindow(false));
        }

        public W_KhachHangModalViewModel(KhachHang kh) : this()
        {
            _isEdit = true;
            _id = kh.ID;
            HoTen = kh.HoTen;
            SoDienThoai = kh.SoDienThoai;
        }

        private void ExecuteSave()
        {
            var kh = new KhachHang
            {
                ID = _id,
                HoTen = HoTen?.Trim(),
                SoDienThoai = SoDienThoai?.Trim()
            };

            if (!ValidationHelper.ValidateWithReport(kh)) return;

            try
            {
                if (_isEdit)
                {
                    _khachHangService.Update(kh);
                    DialogService.ShowMessage("Cập nhật khách hàng thành công.");
                }
                else
                {
                    _khachHangService.Register(kh.HoTen, kh.SoDienThoai);
                    DialogService.ShowMessage("Thêm khách hàng thành công.");
                }
                CloseWindow(true);
            }
            catch (Exception ex)
            {
                DialogService.ShowError("Lỗi: " + ex.Message);
            }
        }

        private void CloseWindow(bool result)
        {
            var window = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.DataContext == this);
            if (window != null)
            {
                window.DialogResult = result;
                window.Close();
            }
        }
    }
}

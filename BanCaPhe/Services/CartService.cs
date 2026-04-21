using BanCaPhe.Models;
using BanCaPhe.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanCaPhe.Services
{
    public class CartService : BaseViewModel
    {
        private readonly KhachHangService _khachHangService;

        private static CartService _instance;
        public static CartService Instance => _instance ??= new CartService();

        private CartService()
        {
            _khachHangService = new KhachHangService();
        }

        private KhachHang _currentKhachHang;
        public KhachHang CurrentKhachHang
        {
            get => _currentKhachHang;
            set
            {
                _currentKhachHang = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PhanTramGiamGia));
                NotifyTongTienChanged();
            }
        }

        public int PhanTramGiamGia => CurrentKhachHang?.PhanTramGiamGia ?? 0;

        public decimal GiamGia => TongTien * (PhanTramGiamGia / 100m);

        public decimal TongPhaiThanhToan => TongTien - GiamGia;

        public ObservableCollection<OrderItem> Items { get; }
            = new ObservableCollection<OrderItem>();

        public decimal TongTien =>
            Items.Sum(i => i.TongTien);

        public void Clear()
        {
            Items.Clear();
            CurrentKhachHang = null;
        }

        public void NotifyTongTienChanged()
        {
            OnPropertyChanged(nameof(TongTien));
            OnPropertyChanged(nameof(GiamGia));
            OnPropertyChanged(nameof(TongPhaiThanhToan));
        }

        public string ApDungKhachHang(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                CurrentKhachHang = null;
                return null;
            }

            try
            {
                var kh = _khachHangService.GetByPhone(phone);
                if (kh == null)
                {
                    CurrentKhachHang = null;
                    return "Không tìm thấy khách hàng này!";
                }

                if (kh.ConDung == false)
                {
                    CurrentKhachHang = null;
                    return "Số điện thoại này đã ngừng sử dụng!";
                }

                CurrentKhachHang = kh;
                return null; // Thành công
            }
            catch (Exception ex)
            {
                return "Lỗi hệ thống: " + ex.Message;
            }
        }

        public void HuyBoKhachHang()
        {
            CurrentKhachHang = null;
        }
    }
}

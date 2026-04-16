using BanCaPhe.Models;
using BanCaPhe.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BanCaPhe.ViewModel
{
    internal class DoanhThuViewModel : BaseViewModel
    {
        private readonly DoanhThuService _service;

        private decimal _tongDoanhThu;
        public decimal TongDoanhThu
        {
            get => _tongDoanhThu;
            set
            {
                _tongDoanhThu = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TongDoanhThuDisplay));
            }
        }

        private int _tongDonHang;
        public int TongDonHang
        {
            get => _tongDonHang;
            set
            {
                _tongDonHang = value;
                OnPropertyChanged();
            }
        }

        private string _thangNamHienTai;
        public string ThangNamHienTai
        {
            get => _thangNamHienTai;
            set
            {
                _thangNamHienTai = value;
                OnPropertyChanged();
            }
        }

        public string TongDoanhThuDisplay => $"{TongDoanhThu:N0} ₫";

        // thống kê Đơn hàng 
        private decimal _doanhThuTrungBinh;
        public decimal DoanhThuTrungBinh
        {
            get => _doanhThuTrungBinh;
            set
            {
                _doanhThuTrungBinh = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DoanhThuTrungBinhDisplay));
            }
        }

        private decimal _donHangLonNhat;
        public decimal DonHangLonNhat
        {
            get => _donHangLonNhat;
            set
            {
                _donHangLonNhat = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DonHangLonNhatDisplay));
            }
        }


        private decimal _donHangNhoNhat;
        public decimal DonHangNhoNhat
        {
            get => _donHangNhoNhat;
            set
            {
                _donHangNhoNhat = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DonHangNhoNhatDisplay));
            }
        }

        public string DoanhThuTrungBinhDisplay => $"{DoanhThuTrungBinh:N0} ₫";
        public string DonHangLonNhatDisplay => $"{DonHangLonNhat:N0} ₫";
        public string DonHangNhoNhatDisplay => $"{DonHangNhoNhat:N0} ₫";


        // sản phẩm bán chạy

        private ObservableCollection<SanPhamBanChayModel> _sanPhamBanChay;
        public ObservableCollection<SanPhamBanChayModel> SanPhamBanChay
        {
            get => _sanPhamBanChay;
            set
            {
                _sanPhamBanChay = value;
                OnPropertyChanged();
            }
        }

        private SanPhamBanChayModel _sanPhamBanChayNhat;
        public SanPhamBanChayModel SanPhamBanChayNhat
        {
            get => _sanPhamBanChayNhat;
            set
            {
                _sanPhamBanChayNhat = value;
                OnPropertyChanged();
            }
        }

        // topping phor biến 

        private ObservableCollection<ToppingPhoBienModel> _toppingPhoBien;
        public ObservableCollection<ToppingPhoBienModel> ToppingPhoBien
        {
            get => _toppingPhoBien;
            set
            {
                _toppingPhoBien = value;
                OnPropertyChanged();
            }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        private string _statusMessage;
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }


        public ICommand LoadDataCommand { get; }
        public ICommand RefreshCommand { get; }


        public DoanhThuViewModel()
        {
            _service = new DoanhThuService();

            SanPhamBanChay = new ObservableCollection<SanPhamBanChayModel>();
            ToppingPhoBien = new ObservableCollection<ToppingPhoBienModel>();

            LoadDataCommand = new RelayCommand(_ => LoadData());
            RefreshCommand = new RelayCommand(_ => RefreshData());

            // Load dữ liệu khi khởi tạo
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                IsLoading = true;
                StatusMessage = "Đang tải dữ liệu...";

                // 1. Load doanh thu tháng này
                var doanhThuThang = _service.GetDoanhThuThangNay();
                if (doanhThuThang != null)
                {
                    TongDoanhThu = doanhThuThang.TongDoanhThu;
                    TongDonHang = doanhThuThang.TongDonHang;
                    ThangNamHienTai = doanhThuThang.ThangNamDisplay;
                }

                // 2. Load thống kê đơn hàng
                var thongKeDonHang = _service.GetTongDonHangThangNay();
                if (thongKeDonHang != null)
                {
                    DoanhThuTrungBinh = thongKeDonHang.DoanhThuTrungBinh;
                    DonHangLonNhat = thongKeDonHang.DonHangLonNhat;
                    DonHangNhoNhat = thongKeDonHang.DonHangNhoNhat;
                }

                // 3. Load sản phẩm bán chạy (top 10)
                var sanPhamList = _service.GetSanPhamBanChayNhat(10);
                SanPhamBanChay.Clear();
                foreach (var sp in sanPhamList)
                {
                    SanPhamBanChay.Add(sp);
                }

                // Lấy sản phẩm bán chạy nhất (top 1)
                if (sanPhamList.Count > 0)
                {
                    SanPhamBanChayNhat = sanPhamList[0];
                }

                // 4. Load topping phổ biến (top 5)
                var toppingList = _service.GetToppingPhoBienNhat(5);
                ToppingPhoBien.Clear();
                foreach (var tp in toppingList)
                {
                    ToppingPhoBien.Add(tp);
                }

                StatusMessage = "Tải dữ liệu thành công!";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Lỗi: {ex.Message}";
                DialogService.ShowError($"Không thể tải dữ liệu: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void RefreshData()
        {
            LoadData();
        }
    }
}

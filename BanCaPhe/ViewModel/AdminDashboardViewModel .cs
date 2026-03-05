using BanCaPhe.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BanCaPhe.ViewModel
{
    class AdminDashboardViewModel : BaseViewModel
    {
        private readonly DoanhThuService _doanhThuService;
        private readonly DoUongService _doUongService;
        private readonly LoaiDoUongService _loaiDoUongService;
        private readonly ToppingService _toppingService;
        private readonly NhanVienService _nhanVienService;


        //

        // doanh thu
        private string _doanhThuDisplay;
        public string DoanhThuDisplay
        {
            get => _doanhThuDisplay;
            set
            {
                _doanhThuDisplay = value;
                OnPropertyChanged();
            }
        }

        // Số Sản Phẩm
        private int _soLuongSanPham;
        public int SoLuongSanPham
        {
            get => _soLuongSanPham;
            set
            {
                _soLuongSanPham = value;
                OnPropertyChanged();
            }
        }

        // Số Loại Sản Phẩm
        private int _soLuongLoaiSanPham;
        public int SoLuongLoaiSanPham
        {
            get => _soLuongLoaiSanPham;
            set
            {
                _soLuongLoaiSanPham = value;
                OnPropertyChanged();
            }
        }

        // Số Topping
        private int _soLuongTopping;
        public int SoLuongTopping
        {
            get => _soLuongTopping;
            set
            {
                _soLuongTopping = value;
                OnPropertyChanged();
            }
        }

        // Số Nhân Viên
        private int _soLuongNhanVien;
        public int SoLuongNhanVien
        {
            get => _soLuongNhanVien;
            set
            {
                _soLuongNhanVien = value;
                OnPropertyChanged();
            }
        }

        public ICommand RefreshDataCommand { get; }

        public AdminDashboardViewModel()
        {
            _doanhThuService = new DoanhThuService();
            _doUongService = new DoUongService();
            _loaiDoUongService = new LoaiDoUongService();
            _toppingService = new ToppingService();
            _nhanVienService = new NhanVienService();

            RefreshDataCommand = new RelayCommand(_ => LoadAllData());

            // Load dữ liệu ban đầu
            LoadAllData();
        }

        public void LoadAllData()
        {
            try
            {
                // 1. Load Doanh Thu
                LoadDoanhThu();

                // 2. Load Số Sản Phẩm
                LoadSoLuongSanPham();

                // 3. Load Số Loại Sản Phẩm
                LoadSoLuongLoaiSanPham();

                // 4. Load Số Topping
                LoadSoLuongTopping();

                // 5. Load Số Nhân Viên
                LoadSoLuongNhanVien();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}",
                    "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadDoanhThu()
        {
            try
            {
                var doanhThu = _doanhThuService.GetDoanhThuThangNay();
                if (doanhThu != null)
                {
                    DoanhThuDisplay = $"{doanhThu.TongDoanhThu:N0} ₫";
                }
                else
                {
                    DoanhThuDisplay = "0 ₫";
                }
            }
            catch
            {
                DoanhThuDisplay = "Lỗi";
            }
        }


        private void LoadSoLuongSanPham()
        {
            try
            {
                var danhSach = _doUongService.GetAll();
                SoLuongSanPham = danhSach?.Count ?? 0;
            }
            catch
            {
                SoLuongSanPham = 0;
            }
        }


        private void LoadSoLuongLoaiSanPham()
        {
            try
            {
                var danhSach = _loaiDoUongService.GetAll();
                SoLuongLoaiSanPham = danhSach?.Count ?? 0;
            }
            catch
            {
                SoLuongLoaiSanPham = 0;
            }
        }

        private void LoadSoLuongTopping()
        {
            try
            {
                var danhSach = _toppingService.GetAll();
                SoLuongTopping = danhSach?.Count ?? 0;
            }
            catch
            {
                SoLuongTopping = 0;
            }
        }

        private void LoadSoLuongNhanVien()
        {
            try
            {
                var danhSach = _nhanVienService.GetAll();
                SoLuongNhanVien = danhSach?.Count ?? 0;
            }
            catch
            {
                SoLuongNhanVien = 0;
            }
        }
    }
}

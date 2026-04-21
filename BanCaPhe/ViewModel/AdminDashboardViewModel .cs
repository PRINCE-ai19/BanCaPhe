using BanCaPhe.Services;
using BanCaPhe.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BanCaPhe.ViewModel
{
    public class AdminDashboardViewModel : BaseViewModel
    {
        private readonly DoanhThuService _doanhThuService;
        private readonly DoUongService _doUongService;
        private readonly LoaiDoUongService _loaiDoUongService;
        private readonly ToppingService _toppingService;
        private readonly NhanVienService _nhanVienService;
        private readonly KhachHangService _khachHangService;

        private object _currentContent;
        public object CurrentContent
        {
            get => _currentContent;
            set { _currentContent = value; OnPropertyChanged(); }
        }


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

        // Số Khách Hàng
        private int _soLuongKhachHang;
        public int SoLuongKhachHang
        {
            get => _soLuongKhachHang;
            set
            {
                _soLuongKhachHang = value;
                OnPropertyChanged();
            }
        }

        public ICommand RefreshDataCommand { get; }
        public ICommand NavigateCommand { get; }
        public ICommand LogoutCommand { get; }

        public AdminDashboardViewModel()
        {
            _doanhThuService = new DoanhThuService();
            _doUongService = new DoUongService();
            _loaiDoUongService = new LoaiDoUongService();
            _toppingService = new ToppingService();
            _nhanVienService = new NhanVienService();
            _khachHangService = new KhachHangService();

            RefreshDataCommand = new RelayCommand(_ => LoadAllData());
            NavigateCommand = new RelayCommand(ExecuteNavigate);
            LogoutCommand = new RelayCommand(ExecuteLogout);

            // Mặc định hiển thị Doanh thu
            ExecuteNavigate("DoanhThu");

            // Load dữ liệu ban đầu
            LoadAllData();
        }

        private void ExecuteNavigate(object param)
        {
            string viewName = param as string;
            switch (viewName)
            {
                case "DoanhThu": CurrentContent = new UC_DoanhThu(); break;
                case "SanPham": CurrentContent = new UC_SanPham(); break;
                case "LoaiSanPham": CurrentContent = new W_LoaiSanPham(); break;
                case "Topping": CurrentContent = new UC_AdminTopping(); break;
                case "NhanVien": CurrentContent = new UC_NhanVien(); break;
                case "KhachHang": CurrentContent = new UC_KhachHang(); break;
            }
            // Refresh stats whenever we switch views
            LoadAllData();
        }

        private void ExecuteLogout(object obj)
        {
            if (DialogService.ShowConfirm("Bạn có chắc muốn đăng xuất?"))
            {
                WindowService.ShowLoginWindow();
                // Đóng dashboard hiện tại
                Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is AdminDashboard)?.Close();
            }
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

                // 6. Load Số Khách Hàng
                LoadSoLuongKhachHang();
            }
            catch (Exception ex)
            {
                DialogService.ShowError($"Lỗi khi tải dữ liệu: {ex.Message}");
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

        private void LoadSoLuongKhachHang()
        {
            try
            {
                // Sử dụng phương thức tìm kiếm rỗng để lấy tất cả khách hàng
                var danhSach = _khachHangService.SearchByPhone("");
                SoLuongKhachHang = danhSach?.Count ?? 0;
            }
            catch
            {
                SoLuongKhachHang = 0;
            }
        }
    }
}

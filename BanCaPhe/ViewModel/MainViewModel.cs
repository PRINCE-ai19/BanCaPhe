using BanCaPhe.Models;
using BanCaPhe.Services;
using BanCaPhe.Views;
using System.Windows;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BanCaPhe.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private readonly DoUongService _sanPhamService;
        private readonly LoaiDoUongService _loaiService;
        private readonly ToppingService _toppingService;

        public ObservableCollection<DanhMucDouong> DanhMuc { get; set; }

        private DanhMucDouong _selectedDanhMuc;
        public DanhMucDouong SelectedDanhMuc
        {
            get => _selectedDanhMuc;
            set
            {
                _selectedDanhMuc = value;
                OnPropertyChanged();
                KieuHienThi = KieuHienThi.DoUong;
                LocSanPhamTheoDanhMuc();
            }
        }

        private ObservableCollection<DoUong> _allSanPham;
        private ObservableCollection<DoUong> _tatCaSanPham;
        public ObservableCollection<DoUong> TatCaSanPham
        {
            get => _tatCaSanPham;
            set
            {
                _tatCaSanPham = value;
                OnPropertyChanged();
                CapNhatDanhSachHienThi();
            }
        }

        private ObservableCollection<Topping> _allTopping;
        public ObservableCollection<Topping> DanhSachTopping
        {
            get => _allTopping;
            set { _allTopping = value; OnPropertyChanged(); }
        }

        private Topping _selectedTopping;
        public Topping SelectedTopping
        {
            get => _selectedTopping;
            set { _selectedTopping = value; OnPropertyChanged(); }
        }

        private KieuHienThi _kieuHienThi;
        public KieuHienThi KieuHienThi
        {
            get => _kieuHienThi;
            set { _kieuHienThi = value; OnPropertyChanged(); CapNhatDanhSachHienThi(); }
        }

        private ObservableCollection<object> _danhSachHienThi;
        public ObservableCollection<object> DanhSachHienThi
        {
            get => _danhSachHienThi;
            set { _danhSachHienThi = value; OnPropertyChanged(); }
        }

        public ObservableCollection<OrderItem> DonHang => CartService.Instance.Items;
        public decimal TongTienGoc => CartService.Instance.TongTien;
        public decimal TongPhaiThanhToan => CartService.Instance.TongPhaiThanhToan;
        public decimal GiamGia => CartService.Instance.GiamGia;
        public int PhanTramGiamGia => CartService.Instance.PhanTramGiamGia;
        public KhachHang KhachHangHienTai => CartService.Instance.CurrentKhachHang;
        public bool HasKhachHang => KhachHangHienTai != null;

        private string _soDienThoaiSearch;
        public string SoDienThoaiSearch
        {
            get => _soDienThoaiSearch;
            set { _soDienThoaiSearch = value; OnPropertyChanged(); }
        }

        private OrderItem _selectedItem;
        public OrderItem SelectedItem
        {
            get => _selectedItem;
            set { _selectedItem = value; OnPropertyChanged(); }
        }

        public ICommand MoGhiChuCommand { get; }
        public ICommand InHoaDonTamCommand { get; }
        public ICommand MoThanhToanCommand { get; }
        public ICommand ChonSanPhamCommand { get; }
        public ICommand ChonToppingCommand { get; }
        public ICommand HienThiToppingCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand HienThiDangKyThanhVienCommand { get; }
        public ICommand TimKhachHangCommand { get; }
        public ICommand HuyKhachHangCommand { get; }

        public MainViewModel()
        {
            _sanPhamService = new DoUongService();
            _loaiService = new LoaiDoUongService();
            _toppingService = new ToppingService();

            DanhMuc = _loaiService.GetAllND();
            _allSanPham = _sanPhamService.GetAllNV();
            TatCaSanPham = new ObservableCollection<DoUong>(_allSanPham);
            _allTopping = new ObservableCollection<Topping>(_toppingService.GetAll());

            KieuHienThi = KieuHienThi.DoUong;
            DanhSachHienThi = new ObservableCollection<object>(TatCaSanPham);

            DonHang.CollectionChanged += (s, e) => NotifyPriceChanged();
            CartService.Instance.PropertyChanged += (s, e) => {
                if (e.PropertyName == nameof(CartService.CurrentKhachHang))
                {
                    OnPropertyChanged(nameof(KhachHangHienTai));
                    OnPropertyChanged(nameof(HasKhachHang));
                    OnPropertyChanged(nameof(PhanTramGiamGia));
                }
                NotifyPriceChanged();
            };

            MoGhiChuCommand = new RelayCommand(OpenGhiChu);
            InHoaDonTamCommand = new RelayCommand(_ => MoHoaDonTam());
            MoThanhToanCommand = new RelayCommand(_ => MoThanhToan());
            ChonSanPhamCommand = new RelayCommand<DoUong>(OpenProductDetail);
            ChonToppingCommand = new RelayCommand<Topping>(OpenToppingDetail);
            HienThiToppingCommand = new RelayCommand(_ => HienThiTopping());
            LogoutCommand = new RelayCommand(_ => Logout());
            HienThiDangKyThanhVienCommand = new RelayCommand(_ => HienThiDangKyThanhVien());
            TimKhachHangCommand = new RelayCommand(_ => TimKhachHang());
            HuyKhachHangCommand = new RelayCommand(_ => HuyKhachHang());
        }

        private void NotifyPriceChanged()
        {
            OnPropertyChanged(nameof(TongTienGoc));
            OnPropertyChanged(nameof(TongPhaiThanhToan));
            OnPropertyChanged(nameof(GiamGia));
        }

        private void TimKhachHang()
        {
            string error = CartService.Instance.ApDungKhachHang(SoDienThoaiSearch);
            if (error != null)
            {
                DialogService.ShowError(error);
            }
            else if (HasKhachHang)
            {
                SoDienThoaiSearch = ""; // Clear search box on success
            }
        }

        private void HuyKhachHang()
        {
            CartService.Instance.HuyBoKhachHang();
        }

        private void LocSanPhamTheoDanhMuc()
        {
            if (SelectedDanhMuc == null) return;
            if (SelectedDanhMuc.TenLoai == "Tất cả đồ uống")
            {
                TatCaSanPham = new ObservableCollection<DoUong>(_allSanPham);
                CapNhatDanhSachHienThi();
                return;
            }
            var filtered = _allSanPham.Where(sp => sp.LoaiID == SelectedDanhMuc.ID).ToList();
            TatCaSanPham = new ObservableCollection<DoUong>(filtered);
            CapNhatDanhSachHienThi();
        }

        private void CapNhatDanhSachHienThi()
        {
            if (KieuHienThi == KieuHienThi.DoUong) DanhSachHienThi = new ObservableCollection<object>(TatCaSanPham);
            else DanhSachHienThi = new ObservableCollection<object>(_allTopping);
        }

        public void HienThiTopping() { KieuHienThi = KieuHienThi.Topping; }

        private void OpenGhiChu(object obj)
        {
            if (SelectedItem == null) return;
            var vm = new GhiChuViewModel(SelectedItem);
            var view = new GhiChuWindow { DataContext = vm };
            view.ShowDialog();
        }

        private void MoHoaDonTam()
        {
            var vm = new HoaDonTamViewModel(CartService.Instance.Items, CartService.Instance.TongTien);
            var view = new HoaDonTamWindow { DataContext = vm };
            view.ShowDialog();
        }

        private void MoThanhToan()
        {
            if (DonHang.Count == 0)
            {
                DialogService.ShowError("Giỏ hàng đang trống!");
                return;
            }
            var vm = new ThanhToanViewModel(TongPhaiThanhToan, DonHang);
            var view = new ThanhToanWindow { DataContext = vm, Owner = Application.Current.MainWindow };
            view.ShowDialog();
        }

        private void OpenProductDetail(DoUong sp)
        {
            if (sp == null) return;
            var sizes = new KichThuocService().GetBySanPhamId(sp.ID);
            var vm = new ProductDetailViewModel(sp, sizes);
            var window = new ProductDetailWindow { DataContext = vm, Owner = Application.Current.MainWindow };
            window.ShowDialog();
        }

        private void OpenToppingDetail(Topping topping)
        {
            if (topping == null) return;
            var vm = new ToppingDetailViewModel(topping);
            var window = new ToppingDetailWindow { DataContext = vm, Owner = Application.Current.MainWindow };
            window.ShowDialog();
        }

        private void Logout()
        {
            var currentWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is MainWindow);
            W_DangNhap login = new W_DangNhap();
            login.Show();
            currentWindow?.Close();
        }

        private void HienThiDangKyThanhVien()
        {
            var view = new W_DangKyThanhVien();
            view.Owner = Application.Current.MainWindow;
            view.ShowDialog();
        }
    }
}

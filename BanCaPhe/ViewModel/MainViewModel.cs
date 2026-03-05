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
using static System.Net.Mime.MediaTypeNames;

namespace BanCaPhe.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private readonly DoUongService _sanPhamService;
        private readonly LoaiDoUongService _loaiService;
        private readonly ToppingService _toppingService;

        //  DANH MỤC 
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

        //  ĐỒ UỐNG
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

        // TOPPING 
        private ObservableCollection<Topping> _allTopping;
        public ObservableCollection<Topping> DanhSachTopping
        {
            get => _allTopping;
            set
            {
                _allTopping = value;
                OnPropertyChanged();
            }
        }

        private Topping _selectedTopping;
        public Topping SelectedTopping
        {
            get => _selectedTopping;
            set
            {
                _selectedTopping = value;
                OnPropertyChanged();
            }
        }

        //  TRẠNG THÁI HIỂN THỊ 
        private KieuHienThi _kieuHienThi;
        public KieuHienThi KieuHienThi
        {
            get => _kieuHienThi;
            set
            {
                _kieuHienThi = value;
                OnPropertyChanged();
                CapNhatDanhSachHienThi();
            }
        }

        // DANH SÁCH HIỂN THỊ CHUNG 
        private ObservableCollection<object> _danhSachHienThi;
        public ObservableCollection<object> DanhSachHienThi
        {
            get => _danhSachHienThi;
            set
            {
                _danhSachHienThi = value;
                OnPropertyChanged();
            }
        }

        // GIỎ HÀNG
        public ObservableCollection<OrderItem> DonHang => CartService.Instance.Items;

        public decimal TongTien => CartService.Instance.TongTien;

        // ĐƠN HÀNG ĐƯỢC CHỌN
        private OrderItem _selectedItem;
        public OrderItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
            }
        }

        public ICommand MoGhiChuCommand { get; }

        public ICommand InHoaDonTamCommand { get; }

        public ICommand MoThanhToanCommand { get; }

      


        public MainViewModel()
        {
            _sanPhamService = new DoUongService();
            _loaiService = new LoaiDoUongService();
            _toppingService = new ToppingService();

            // Danh mục
            DanhMuc = _loaiService.GetAllND();

            // Đồ uống
            _allSanPham = _sanPhamService.GetAllNV();
            TatCaSanPham = new ObservableCollection<DoUong>(_allSanPham);

            // Topping
            _allTopping = new ObservableCollection<Topping>(_toppingService.GetAll());

            // Mặc định
            KieuHienThi = KieuHienThi.DoUong;
            DanhSachHienThi = new ObservableCollection<object>(TatCaSanPham);

            DonHang.CollectionChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(TongTien));
            };

          

            //    ghi chú
            MoGhiChuCommand = new RelayCommand(OpenGhiChu);

            // in hóa đơn tạm
            InHoaDonTamCommand = new RelayCommand(_ => MoHoaDonTam());

            // mở thanh toán
            MoThanhToanCommand = new RelayCommand(_ => MoThanhToan());

           


        }

        // LỌC ĐỒ UỐNG 
        private void LocSanPhamTheoDanhMuc()
        {
            if (SelectedDanhMuc == null)
            { 
                return;
            }

            if (SelectedDanhMuc.TenLoai == "Tất cả đồ uống")
            {
                TatCaSanPham = new ObservableCollection<DoUong>(_allSanPham);
                CapNhatDanhSachHienThi();
                return;
            }

            var filtered = _allSanPham
                .Where(sp => sp.LoaiID == SelectedDanhMuc.ID)
                .ToList();

            TatCaSanPham = new ObservableCollection<DoUong>(filtered);
            CapNhatDanhSachHienThi();
        }

        //  CẬP NHẬT DANH SÁCH HIỂN THỊ
        private void CapNhatDanhSachHienThi()
        {
            if (KieuHienThi == KieuHienThi.DoUong)
            {
                DanhSachHienThi = new ObservableCollection<object>(TatCaSanPham);
            }
            else
            {
                DanhSachHienThi = new ObservableCollection<object>(_allTopping);
            }
        }

        //  GỌI KHI CLICK "TOPPING" 
        public void HienThiTopping()
        {
            KieuHienThi = KieuHienThi.Topping;
        }

        // MỞ GHI CHÚ
        private void OpenGhiChu(object obj)
        {
            if (SelectedItem == null) return;

            var vm = new GhiChuViewModel(SelectedItem);
            var view = new GhiChuWindow
            {
                DataContext = vm,
            
            };

            view.ShowDialog();
        }

        private void MoHoaDonTam()
        {
            var vm = new HoaDonTamViewModel(
                CartService.Instance.Items,
                CartService.Instance.TongTien
            );

            var view = new HoaDonTamWindow
            {
                DataContext = vm
            };

            view.ShowDialog();
        }

        private void MoThanhToan()
        {
            var vm = new ThanhToanViewModel(TongTien, DonHang);

            var view = new ThanhToanWindow
            {
                DataContext = vm,
                Owner = System.Windows.Application.Current.MainWindow
            };

            view.ShowDialog();
        }

       

    }
}

using BanCaPhe.Helpers;
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
    public class ProductDetailViewModel : BaseViewModel
    {
        public DoUong SanPham { get; set; }
        public ObservableCollection<KichThuoc> DanhSachKichThuoc { get; }
        public ObservableCollection<Topping> DanhSachTopping { get; }

        public bool LaTraSua
        {
            get
            {
                var loaiService = new LoaiDoUongService();
                var loai = loaiService.GetAllND().FirstOrDefault(l => l.ID == SanPham.LoaiID);
                return loai != null ? loai.TenLoai.ToLower().Contains("trà sữa") : SanPham.TenDoUong.ToLower().Contains("trà sữa");
            }
        }

        private KichThuoc _kichThuocDuocChon;
        public KichThuoc KichThuocDuocChon
        {
            get => _kichThuocDuocChon;
            set
            {
                _kichThuocDuocChon = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(GiaHienTai));
                OnPropertyChanged(nameof(TongTien));
            }
        }

        public decimal GiaGoc => SanPham.Gia;
        public decimal GiaHienTai => GiaGoc + (KichThuocDuocChon?.Gia ?? 0);
        public decimal TongTien
        {
            get
            {
                decimal tong = GiaHienTai * SoLuong;
                foreach (var topping in DanhSachToppingDaChon) tong += topping.Gia * topping.SoLuong;
                return tong;
            }
        }

        private int _soLuong = 1;
        public int SoLuong
        {
            get => _soLuong;
            set
            {
                _soLuong = value < 1 ? 1 : value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TongTien));
            }
        }

        public ObservableCollection<ToppingItem> DanhSachToppingDaChon { get; set; } = new ObservableCollection<ToppingItem>();

        private Topping _selectedTopping;
        public Topping SelectedTopping
        {
            get => _selectedTopping;
            set { _selectedTopping = value; OnPropertyChanged(); }
        }

        private int _soLuongTopping = 1;
        public int SoLuongTopping
        {
            get => _soLuongTopping;
            set { _soLuongTopping = value < 1 ? 1 : value; OnPropertyChanged(); }
        }

        public ICommand ChonSizeCommand { get; }
        public ICommand TangSoLuongCommand { get; }
        public ICommand GiamSoLuongCommand { get; }
        public ICommand ThemVaoGioCommand { get; }
        public ICommand ThemToppingCommand { get; }
        public ICommand XoaToppingCommand { get; }
        public ICommand CloseCommand { get; }

        public ProductDetailViewModel(DoUong sp, List<KichThuoc> sizes)
        {
            SanPham = sp;
            DanhSachKichThuoc = new ObservableCollection<KichThuoc>(sizes);
            DanhSachTopping = new ObservableCollection<Topping>(new ToppingService().GetAll());

            var sizeMacDinh = DanhSachKichThuoc.FirstOrDefault();
            if (sizeMacDinh != null) { sizeMacDinh.IsSelected = true; KichThuocDuocChon = sizeMacDinh; }

            ChonSizeCommand = new RelayCommand<KichThuoc>(size => {
                foreach (var s in DanhSachKichThuoc) s.IsSelected = false;
                size.IsSelected = true;
                KichThuocDuocChon = size;
            });

            TangSoLuongCommand = new RelayCommand(_ => SoLuong++);
            GiamSoLuongCommand = new RelayCommand(_ => SoLuong--);
            ThemVaoGioCommand = new RelayCommand(_ => ThemVaoGio());
            ThemToppingCommand = new RelayCommand(_ => ThemTopping());
            XoaToppingCommand = new RelayCommand<ToppingItem>(topping => XoaTopping(topping));
            CloseCommand = new RelayCommand(_ => CloseWindow(false));

            DanhSachToppingDaChon.CollectionChanged += (s, e) => OnPropertyChanged(nameof(TongTien));
        }

        private void ThemTopping()
        {
            if (SelectedTopping == null) { DialogService.ShowError("Vui lòng chọn topping!"); return; }
            var exist = DanhSachToppingDaChon.FirstOrDefault(x => x.ToppingID == SelectedTopping.ID);
            if (exist != null) exist.SoLuong += SoLuongTopping;
            else DanhSachToppingDaChon.Add(new ToppingItem { ToppingID = SelectedTopping.ID, Ten = SelectedTopping.TenTopping, Gia = SelectedTopping.Gia, SoLuong = SoLuongTopping });
            SoLuongTopping = 1;
            OnPropertyChanged(nameof(TongTien));
        }

        private void XoaTopping(ToppingItem topping)
        {
            if (topping != null) { DanhSachToppingDaChon.Remove(topping); OnPropertyChanged(nameof(TongTien)); }
        }

        private void ThemVaoGio()
        {
            if (KichThuocDuocChon == null) { DialogService.ShowError("Vui lòng chọn size!"); return; }
            var orderItem = new OrderItem { SanPhamKichThuocID = KichThuocDuocChon.ID, Ten = SanPham.TenDoUong, Size = KichThuocDuocChon.TenKichThuoc, SoLuong = SoLuong, DonGia = GiaHienTai, LaTraSua = LaTraSua };
            foreach (var topping in DanhSachToppingDaChon) orderItem.Toppings.Add(new ToppingItem { ToppingID = topping.ToppingID, Ten = topping.Ten, Gia = topping.Gia, SoLuong = topping.SoLuong });
            CartService.Instance.Items.Add(orderItem);
            DialogService.ShowMessage("Đã thêm vào giỏ hàng!");
            CloseWindow(true);
        }

        private void CloseWindow(bool result)
        {
            var window = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.DataContext == this);
            if (window != null) { window.DialogResult = result; window.Close(); }
        }
    }
}

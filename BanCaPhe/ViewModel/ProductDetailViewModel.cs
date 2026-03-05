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
                // Kiểm tra theo tên loại từ database
                var loaiService = new LoaiDoUongService();
                var loai = loaiService.GetAllND().FirstOrDefault(l => l.ID == SanPham.LoaiID);

                if (loai != null)
                {
                    return loai.TenLoai.ToLower().Contains("trà sữa");
                }

                // Fallback: kiểm tra theo tên sản phẩm
                return SanPham.TenDoUong.ToLower().Contains("trà sữa");
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

        // GIÁ 
        public decimal GiaGoc => SanPham.Gia;

        public decimal GiaHienTai =>
            GiaGoc + (KichThuocDuocChon?.Gia ?? 0);

        public decimal TongTien
        {
            get
            {
                decimal tong = GiaHienTai * SoLuong;
                // Cộng thêm giá topping
                foreach (var topping in DanhSachToppingDaChon)
                {
                    tong += topping.Gia * topping.SoLuong;
                }
                return tong;
            }
        }
        //  SỐ LƯỢNG 
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


        // DANH SÁCH TOPPING ĐÃ CHỌN
        public ObservableCollection<ToppingItem> DanhSachToppingDaChon { get; set; }
            = new ObservableCollection<ToppingItem>();

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

        private int _soLuongTopping = 1;
        public int SoLuongTopping
        {
            get => _soLuongTopping;
            set
            {
                _soLuongTopping = value < 1 ? 1 : value;
                OnPropertyChanged();
            }
        }



        public ICommand ChonSizeCommand { get; }
        public ICommand TangSoLuongCommand { get; }
        public ICommand GiamSoLuongCommand { get; }
        public ICommand ThemVaoGioCommand { get; }

        public ICommand ThemToppingCommand { get; }
        public ICommand XoaToppingCommand { get; }



        public ProductDetailViewModel(DoUong sp, List<KichThuoc> sizes)
        {
            SanPham = sp;
            DanhSachKichThuoc = new ObservableCollection<KichThuoc>(sizes);

            // Load danh sách topping
            var toppingService = new ToppingService();
            DanhSachTopping = new ObservableCollection<Topping>(toppingService.GetAll());


            // Mặc định chọn size đầu tiên (S)
            var sizeMacDinh = DanhSachKichThuoc.FirstOrDefault();
            if (sizeMacDinh != null)
            {
                sizeMacDinh.IsSelected = true;
                KichThuocDuocChon = sizeMacDinh;
            }

            ChonSizeCommand = new RelayCommand<KichThuoc>(size =>
            {
                foreach (var s in DanhSachKichThuoc)
                    s.IsSelected = false;

                size.IsSelected = true;
                KichThuocDuocChon = size;
            });

            TangSoLuongCommand = new RelayCommand(_ => SoLuong++);
            GiamSoLuongCommand = new RelayCommand(_ => SoLuong--);

            ThemVaoGioCommand = new RelayCommand(_ => ThemVaoGio());

            ThemToppingCommand = new RelayCommand(_ => ThemTopping());
            XoaToppingCommand = new RelayCommand<ToppingItem>(topping => XoaTopping(topping));

            // Cập nhật tổng tiền khi danh sách topping thay đổi
            DanhSachToppingDaChon.CollectionChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(TongTien));
            };
        }

        // THÊM TOPPING
        private void ThemTopping()
        {
            if (SelectedTopping == null)
            {
                MessageBox.Show("Vui lòng chọn topping!", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var exist = DanhSachToppingDaChon
                .FirstOrDefault(x => x.ToppingID == SelectedTopping.ID);

            if (exist != null)
            {
                exist.SoLuong += SoLuongTopping;
            }
            else
            {
                DanhSachToppingDaChon.Add(new ToppingItem
                {
                    ToppingID = SelectedTopping.ID,
                    Ten = SelectedTopping.TenTopping,
                    Gia = SelectedTopping.Gia,
                    SoLuong = SoLuongTopping
                });
            }

            // Reset số lượng topping về 1
            SoLuongTopping = 1;
            OnPropertyChanged(nameof(TongTien));
        }


        // XÓA TOPPING
        private void XoaTopping(ToppingItem topping)
        {
            if (topping != null)
            {
                DanhSachToppingDaChon.Remove(topping);
                OnPropertyChanged(nameof(TongTien));
            }
        }

        // THÊM VÀO GIỎ 
        private void ThemVaoGio()
        {

            if (KichThuocDuocChon == null)
            {
                MessageBox.Show("Vui lòng chọn size!",
                    "Thiếu thông tin",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            var orderItem = new OrderItem
            {
                SanPhamKichThuocID = KichThuocDuocChon.ID,
                Ten = SanPham.TenDoUong,
                Size = KichThuocDuocChon.TenKichThuoc,
                SoLuong = SoLuong,
                DonGia = GiaHienTai,
                LaTraSua = LaTraSua
            };

            // Thêm các topping đã chọn vào đơn hàng
            foreach (var topping in DanhSachToppingDaChon)
            {
                orderItem.Toppings.Add(new ToppingItem
                {
                    ToppingID = topping.ToppingID,
                    Ten = topping.Ten,
                    Gia = topping.Gia,
                    SoLuong = topping.SoLuong
                });
            }

            CartService.Instance.Items.Add(orderItem);

            MessageBox.Show("Đã thêm vào giỏ hàng!", "Thành công",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}

using BanCaPhe.Models;
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
    public class ThanhToanTienMatViewModel : BaseViewModel
    {
        private readonly DonHangService _donHangService;

        public decimal TongTienGoc { get; }
        
        private decimal _tongTien;
        public decimal TongTien
        {
            get => _tongTien;
            private set
            {
                _tongTien = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TienThoi));
            }
        }

        private string _soDienThoai;
        public string SoDienThoai
        {
            get => _soDienThoai;
            set
            {
                _soDienThoai = value;
                OnPropertyChanged();
            }
        }

        private KhachHang _khachHang;
        public KhachHang KhachHang
        {
            get => _khachHang;
            set
            {
                _khachHang = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasKhachHang));
                TinhToanGiamGia();
            }
        }

        public bool HasKhachHang => KhachHang != null;

        private decimal _giamGia;
        public decimal GiamGia
        {
            get => _giamGia;
            set
            {
                _giamGia = value;
                OnPropertyChanged();
            }
        }

        private string _tienKhachDuaText = "0";
        public string TienKhachDuaText
        {
            get => _tienKhachDuaText;
            set
            {
                _tienKhachDuaText = value;
                OnPropertyChanged();

                // Parse và cập nhật TienKhachDua
                if (decimal.TryParse(value.Replace(",", "").Replace(".", ""), out decimal tien))
                {
                    TienKhachDua = tien;
                }
            }
        }

        private decimal _tienKhachDua;
        public decimal TienKhachDua
        {
            get => _tienKhachDua;
            set
            {
                _tienKhachDua = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TienThoi));
            }
        }

        public decimal TienThoi
        {
            get
            {
                if (TienKhachDua < TongTien)
                    return 0;

                return TienKhachDua - TongTien;
            }
        }

        // Commands
        public ICommand NhapSoCommand { get; }
        public ICommand XoaCommand { get; }
        public ICommand XoaHetCommand { get; }
        public ICommand VuaDuCommand { get; }
        public ICommand ChonMenhGiaCommand { get; }
        public ICommand XacNhanCommand { get; }
        public ICommand HuyCommand { get; }
        public ICommand TimKhachHangCommand { get; }

        public ThanhToanTienMatViewModel(decimal tongTien)
        {
            TongTienGoc = tongTien;
            TongTien = tongTien;

            _donHangService = new DonHangService();

            // Tự động lấy khách hàng từ giỏ hàng nếu có
            if (CartService.Instance.CurrentKhachHang != null)
            {
                KhachHang = CartService.Instance.CurrentKhachHang;
                SoDienThoai = KhachHang.SoDienThoai;
                // TinhToanGiamGia sẽ được gọi qua Setter của KhachHang
            }

            // Khởi tạo commands
            NhapSoCommand = new RelayCommand(NhapSo);
            XoaCommand = new RelayCommand(_ => Xoa());
            XoaHetCommand = new RelayCommand(_ => XoaHet());
            VuaDuCommand = new RelayCommand(_ => VuaDu());
            ChonMenhGiaCommand = new RelayCommand(ChonMenhGia);
            XacNhanCommand = new RelayCommand(_ => XacNhan());
            HuyCommand = new RelayCommand(_ => Dong());
            TimKhachHangCommand = new RelayCommand(_ => TimKhachHang());
        }

        private void TimKhachHang()
        {
            if (string.IsNullOrEmpty(SoDienThoai))
            {
                KhachHang = null;
                return;
            }

            var service = new KhachHangService();
            var kh = service.GetByPhone(SoDienThoai);

            if (kh != null)
            {
                if (kh.ConDung == false)
                {
                    KhachHang = null;
                    DialogService.ShowError("Số điện thoại này đã không còn được sử dụng nữa!");
                    return;
                }

                KhachHang = kh;
            }
            else
            {
                KhachHang = null;
                DialogService.ShowError("Không tìm thấy khách hàng!");
            }
        }

        private void TinhToanGiamGia()
        {
            if (KhachHang != null)
            {
                GiamGia = (TongTienGoc * KhachHang.PhanTramGiamGia) / 100;
            }
            else
            {
                GiamGia = 0;
            }
            TongTien = TongTienGoc - GiamGia;
        }

        // Nhập số từ bàn phím
        private void NhapSo(object param)
        {
            string so = param?.ToString() ?? "";

            if (_tienKhachDuaText == "0")
                _tienKhachDuaText = "";

            TienKhachDuaText = _tienKhachDuaText + so;
        }


        // Xóa từng số (Backspace)
        private void Xoa()
        {
            if (_tienKhachDuaText.Length > 0)
            {
                TienKhachDuaText = _tienKhachDuaText.Substring(0, _tienKhachDuaText.Length - 1);
                if (string.IsNullOrEmpty(_tienKhachDuaText))
                    TienKhachDuaText = "0";
            }
        }



        // Xóa hết (Clear)
        private void XoaHet()
        {
            TienKhachDuaText = "0";
        }

        // Điền đúng số tiền cần thanh toán
        private void VuaDu()
        {
            TienKhachDuaText = TongTien.ToString("0");
        }

        // Chọn mệnh giá tiền (cộng dồn)
        private void ChonMenhGia(object param)
        {
            if (decimal.TryParse(param?.ToString(), out decimal menhGia))
            {
                decimal tienHienTai = TienKhachDua;
                TienKhachDuaText = (tienHienTai + menhGia).ToString("0");
            }
        }

        private void XacNhan()
        {
            if (TienKhachDua <= 0)
            {
                DialogService.ShowError("Vui lòng nhập tiền khách đưa");
                return;
            }

            if (TienKhachDua < TongTien)
            {
                DialogService.ShowError("Tiền khách đưa không đủ!");
                return;
            }

            try
            {
                // 1️⃣ LẤY THÔNG TIN NHÂN VIÊN ĐANG ĐĂNG NHẬP
                var currentUser = UserSession.CurrentUser;

                if (currentUser == null)
                {
                    DialogService.ShowError("Không tìm thấy thông tin nhân viên đăng nhập!");
                    return;
                }

                // 2️⃣ Tạo đơn hàng
                var donHang = new DonHang
                {
                    NgayLap = DateTime.Now,
                    NhanVienID = currentUser.ID, // ✅ Lấy từ UserSession
                    TongTien = TongTien,
                    HinhThucThanhToan = "Tien mat",
                    KhachHangID = KhachHang?.ID
                };

                // 3️⃣ LẤY GIỎ HÀNG
                var items = CartService.Instance.Items.ToList();

                if (!items.Any())
                {
                    DialogService.ShowError("Giỏ hàng trống!");
                    return;
                }

                // 🔒 4️⃣ VALIDATE DỮ LIỆU TRƯỚC KHI LƯU DB
                foreach (var item in items)
                {
                    if (item.SanPhamKichThuocID <= 0)
                    {
                        DialogService.ShowError($"Sản phẩm '{item.Ten}' chưa có size hợp lệ!");
                        return;
                    }

                    if (item.DonGia <= 0)
                    {
                        DialogService.ShowError($"Sản phẩm '{item.Ten}' có đơn giá không hợp lệ!");
                        return;
                    }

                    // ✔ validate topping (nếu có)
                    if (item.Toppings != null)
                    {
                        foreach (var tp in item.Toppings)
                        {
                            if (tp.ToppingID <= 0 || tp.Gia < 0)
                            {
                                DialogService.ShowError($"Topping của '{item.Ten}' không hợp lệ!");
                                return;
                            }
                        }
                    }
                }

                // 5️⃣ GỌI THANH TOÁN → LƯU DB (DONHANG + CHITIET + TOPPING)
                _donHangService.ThanhToan(donHang, items);

                // 6️⃣ THÀNH CÔNG
                DialogService.ShowMessage($"Thanh toán thành công!\nTiền thối: {TienThoi:N0} đ");

                // 7️⃣ CLEAR GIỎ
                CartService.Instance.Clear();
                CartService.Instance.NotifyTongTienChanged();

                Dong();
            }
            catch (Exception ex)
            {
                DialogService.ShowError("Lỗi thanh toán!\n" + ex.Message);
            }
        }


        private void Dong()
        {
            Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w.IsActive)
                ?.Close();
        }
    }
}

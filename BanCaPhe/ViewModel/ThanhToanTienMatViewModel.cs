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

        public decimal TongTien { get; }

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

        public ThanhToanTienMatViewModel(decimal tongTien)
        {
            TongTien = tongTien;

            _donHangService = new DonHangService();

            // Khởi tạo commands
            NhapSoCommand = new RelayCommand(NhapSo);
            XoaCommand = new RelayCommand(_ => Xoa());
            XoaHetCommand = new RelayCommand(_ => XoaHet());
            VuaDuCommand = new RelayCommand(_ => VuaDu());
            ChonMenhGiaCommand = new RelayCommand(ChonMenhGia);
            XacNhanCommand = new RelayCommand(_ => XacNhan());
            HuyCommand = new RelayCommand(_ => Dong());
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
                MessageBox.Show("Vui lòng nhập tiền khách đưa",
                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (TienKhachDua < TongTien)
            {
                MessageBox.Show("Tiền khách đưa không đủ!",
                    "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // 1️⃣ LẤY THÔNG TIN NHÂN VIÊN ĐANG ĐĂNG NHẬP
                var currentUser = UserSession.CurrentUser;

                if (currentUser == null)
                {
                    MessageBox.Show(
                        "Không tìm thấy thông tin nhân viên đăng nhập!",
                        "Lỗi",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                    return;
                }

                // 2️⃣ Tạo đơn hàng
                var donHang = new DonHang
                {
                    NgayLap = DateTime.Now,
                    NhanVienID = currentUser.ID, // ✅ Lấy từ UserSession
                    TongTien = TongTien,
                    HinhThucThanhToan = "Tien mat"
                };

                // 3️⃣ LẤY GIỎ HÀNG
                var items = CartService.Instance.Items.ToList();

                if (!items.Any())
                {
                    MessageBox.Show("Giỏ hàng trống!",
                        "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // 🔒 4️⃣ VALIDATE DỮ LIỆU TRƯỚC KHI LƯU DB
                foreach (var item in items)
                {
                    if (item.SanPhamKichThuocID <= 0)
                    {
                        MessageBox.Show(
                            $"Sản phẩm '{item.Ten}' chưa có size hợp lệ!",
                            "Lỗi dữ liệu",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error
                        );
                        return;
                    }

                    if (item.DonGia <= 0)
                    {
                        MessageBox.Show(
                            $"Sản phẩm '{item.Ten}' có đơn giá không hợp lệ!",
                            "Lỗi dữ liệu",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error
                        );
                        return;
                    }

                    // ✔ validate topping (nếu có)
                    if (item.Toppings != null)
                    {
                        foreach (var tp in item.Toppings)
                        {
                            if (tp.ToppingID <= 0 || tp.Gia < 0)
                            {
                                MessageBox.Show(
                                    $"Topping của '{item.Ten}' không hợp lệ!",
                                    "Lỗi dữ liệu",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error
                                );
                                return;
                            }
                        }
                    }
                }

                // 5️⃣ GỌI THANH TOÁN → LƯU DB (DONHANG + CHITIET + TOPPING)
                _donHangService.ThanhToan(donHang, items);

                // 6️⃣ THÀNH CÔNG
                MessageBox.Show(
                    $"Thanh toán thành công!\nTiền thối: {TienThoi:N0} đ",
                    "Thành công",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

                // 7️⃣ CLEAR GIỎ
                CartService.Instance.Items.Clear();
                CartService.Instance.NotifyTongTienChanged();

                Dong();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi thanh toán!\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
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

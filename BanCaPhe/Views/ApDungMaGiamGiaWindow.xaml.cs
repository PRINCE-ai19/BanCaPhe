using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BanCaPhe.Views
{
    public partial class ApDungMaGiamGiaWindow : Window
    {
        // Mock data cho demo UI
        private object selectedVoucher = null;

        public ApDungMaGiamGiaWindow()
        {
            InitializeComponent();
            pnlEmptyVoucher.Visibility = Visibility.Visible;
        }

        private void BtnTimKiem_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(keyword))
            {
                MessageBox.Show("Vui lòng nhập họ tên hoặc số điện thoại!", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // TODO: Gọi service tìm kiếm khách hàng
            // var khachHangService = new KhachHangService();
            // var results = khachHangService.Search(keyword);
            // dgKhachHang.ItemsSource = results;

            // Mock data để test UI
            var mockData = new[]
            {
                new { HoTen = "Nguyễn Văn A", SoDienThoai = "0901234567", DiemTichLuy = 150 },
                new { HoTen = "Trần Thị B", SoDienThoai = "0912345678", DiemTichLuy = 320 },
                new { HoTen = "Lê Văn C", SoDienThoai = "0923456789", DiemTichLuy = 80 }
            };

            dgKhachHang.ItemsSource = mockData;
        }

        private void DgKhachHang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgKhachHang.SelectedItem == null)
            {
                pnlEmptyVoucher.Visibility = Visibility.Visible;
                icVouchers.ItemsSource = null;
                btnXacNhan.IsEnabled = false;
                return;
            }

            // TODO: Lấy voucher của khách hàng đã chọn
            // var khachHang = dgKhachHang.SelectedItem as KhachHang;
            // var voucherService = new MaGiamGiaService();
            // var vouchers = voucherService.GetByKhachHangId(khachHang.ID, tongTien);
            // icVouchers.ItemsSource = vouchers;

            // Mock data voucher để test UI
            var mockVouchers = new[]
            {
                new {
                    MaCode = "MODTRA10",
                    TenChuongTrinh = "Giảm 10% cho khách hàng thân thiết",
                    GiaTriGiamDisplay = "10%",
                    DieuKienDisplay = "Đơn tối thiểu 50,000đ"
                },
                new {
                    MaCode = "FREESHIP",
                    TenChuongTrinh = "Miễn phí giao hàng",
                    GiaTriGiamDisplay = "20,000đ",
                    DieuKienDisplay = "Đơn tối thiểu 100,000đ"
                },
                new {
                    MaCode = "SUMMER2026",
                    TenChuongTrinh = "Khuyến mãi mùa hè",
                    GiaTriGiamDisplay = "15%",
                    DieuKienDisplay = "Đơn tối thiểu 200,000đ"
                }
            };

            icVouchers.ItemsSource = mockVouchers;
            pnlEmptyVoucher.Visibility = Visibility.Collapsed;
        }

        private void Voucher_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border)
            {
                selectedVoucher = border.Tag;
                btnXacNhan.IsEnabled = true;

                // Visual feedback
                border.BorderBrush = (System.Windows.Media.Brush)FindResource("SuccessBrush");
                border.BorderThickness = new Thickness(2);
            }
        }

        private void BtnXacNhan_Click(object sender, RoutedEventArgs e)
        {
            if (selectedVoucher == null)
            {
                MessageBox.Show("Vui lòng chọn mã giảm giá!", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // TODO: Tính toán giá trị giảm thực tế
            // var maGiamGiaService = new MaGiamGiaService();
            // var soTienGiam = maGiamGiaService.TinhGiaTriGiam(selectedVoucher, tongTien);

            // Mock data để test UI
            decimal tongTienGoc = 150000;
            decimal soTienGiam = 15000;
            decimal tongThanhToan = tongTienGoc - soTienGiam;

            // Mở modal xác nhận
            var confirmWindow = new XacNhanVoucherWindow(
                "MODTRA10",
                "Giảm 10% cho khách hàng thân thiết",
                "10%",
                tongTienGoc,
                soTienGiam,
                tongThanhToan
            )
            {
                Owner = this
            };

            if (confirmWindow.ShowDialog() == true)
            {
                // TODO: Áp dụng voucher vào đơn hàng
                // var maGiamGiaService = new MaGiamGiaService();
                // maGiamGiaService.ApDung(selectedVoucher);

                MessageBox.Show("Áp dụng mã giảm giá thành công! ✅", "Thành công",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                this.DialogResult = true;
                this.Close();
            }
        }

        private void BtnDong_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}

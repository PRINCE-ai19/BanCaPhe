using BanCaPhe.Helpers;
using BanCaPhe.Models;
using BanCaPhe.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BanCaPhe.Views
{
    /// <summary>
    /// Interaction logic for W_DoiMatKhauModal.xaml
    /// </summary>
    public partial class W_DoiMatKhauModal : Window
    {
        private readonly NhanVienService _service;
        private readonly int _nhanVienId;

        public W_DoiMatKhauModal(int nhanVienId, string hoTen)
        {
            InitializeComponent();
            _service = new NhanVienService();
            _nhanVienId = nhanVienId;

            txtInfo.Text = $"Đổi mật khẩu cho: {hoTen}";
        }

        private void BtnDoiMatKhau_Click(object sender, RoutedEventArgs e)
        {
            // Validate
            if (string.IsNullOrWhiteSpace(txtMatKhauMoi.Password))
            {
                MessageBox.Show("Mật khẩu mới không được để trống");
                return;
            }

            if (txtMatKhauMoi.Password.Length < 6 || txtMatKhauMoi.Password.Length > 50)
            {
                MessageBox.Show("Mật khẩu phải từ 6 đến 50 ký tự");
                return;
            }

            if (txtMatKhauMoi.Password != txtXacNhan.Password)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp");
                return;
            }

            // Hash và update
            string hashedPassword = PasswordHelper.HashPassword(txtMatKhauMoi.Password);
            bool result = _service.UpdatePassword(_nhanVienId, hashedPassword);

            if (result)
            {
                MessageBox.Show("Đổi mật khẩu thành công!", "Thành công",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Đổi mật khẩu thất bại!", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnHuy_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

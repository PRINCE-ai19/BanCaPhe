using BanCaPhe.Models;
using BanCaPhe.Services;
using BanCaPhe.Views;
using Microsoft.Identity.Client;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BanCaPhe
{
    /// <summary>
    /// Interaction logic for UC_NhanVien.xaml
    /// </summary>
    public partial class UC_NhanVien : UserControl
    {

        private readonly NhanVienService _service;
        public UC_NhanVien()
        {
            InitializeComponent();
            _service = new NhanVienService();
            LoadData();
        }

        private void LoadData()
        {
            dgNhanVien.ItemsSource = _service.GetAll();
        }

        private void BtnThem_Click(object sender, RoutedEventArgs e)
        {
            var win = new W_NhanVienModal(); // cửa sổ thêm
            if (win.ShowDialog() == true)
            {
                LoadData();
                MessageBox.Show("Thêm nhân viên thành công!");
            }
        }

        private void BtnSua_Click(object sender, RoutedEventArgs e)
        {
            if (dgNhanVien.SelectedItem is NhanVien nv)
            {
                var dialog = new W_NhanVienModal(nv.ID);
                if (dialog.ShowDialog() == true)
                {
                    LoadData();
                    MessageBox.Show("Cập nhật nhân viên thành công!");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần sửa");
            }
        }

        // NÚT XÓA (MỚI)
        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (dgNhanVien.SelectedItem is NhanVien nv)
            {
                var result = MessageBox.Show(
                    $"Bạn có chắc muốn xóa nhân viên '{nv.HoTen}'?",
                    "Xác nhận xóa",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (result == MessageBoxResult.Yes)
                {
                    if (_service.Delete(nv.ID))
                    {
                        LoadData();
                        MessageBox.Show("Xóa nhân viên thành công!");
                    }
                    else
                    {
                        MessageBox.Show("Xóa nhân viên thất bại!", "Lỗi",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần xóa");
            }
        }


        // NÚT ĐỔI MẬT KHẨU (MỚI)
        private void BtnDoiMatKhau_Click(object sender, RoutedEventArgs e)
        {
            if (dgNhanVien.SelectedItem is NhanVien nv)
            {
                var dialog = new W_DoiMatKhauModal(nv.ID, nv.HoTen);
                dialog.ShowDialog();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần đổi mật khẩu");
            }
        }

    }
}

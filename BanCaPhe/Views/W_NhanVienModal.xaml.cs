using BanCaPhe.Helpers;
using BanCaPhe.Models;
using BanCaPhe.Services;
using Microsoft.Identity.Client;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
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
   
    public partial class W_NhanVienModal : Window
    {
        private readonly NhanVienService _service;
        private int? _id = null;
        private string _hinhAnh = null;


        public W_NhanVienModal()
        {
         
            InitializeComponent();
            _service = new NhanVienService();

            pnlMatKhau.Visibility = Visibility.Visible;

            cbVaiTro.SelectedIndex = 1;
        }

        public W_NhanVienModal(int id) : this()
        {
            _id = id;
            LoadNhanVien();

            pnlMatKhau.Visibility = Visibility.Collapsed;
        }

        private void LoadNhanVien()
        {
            var nv = _service.GetById(_id.Value);
            if (nv == null) return;

            txtHoTen.Text = nv.HoTen;
            txtEmail.Text = nv.Email;
            txtSoDienThoai.Text = nv.SoDienThoai;
            cbVaiTro.Text = nv.VaiTro;
            chkTrangThai.IsChecked = nv.TrangThai;
            _hinhAnh = nv.HinhAnh;



            // Load hình ảnh nếu có
            if (!string.IsNullOrEmpty(_hinhAnh))
            {
                string path = System.IO.Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "HinhAnh",
                    _hinhAnh
                );

                if (File.Exists(path))
                {
                    imgPreview.Source = new BitmapImage(new Uri(path));
                }
            }
        }

        private void BtnChonAnh_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                Filter = "Image (*.jpg;*.png)|*.jpg;*.png"
            };

            if (dlg.ShowDialog() == true)
            {
                // Thư mục HinhAnh trong project
                string folder = System.IO.Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "HinhAnh"
                );

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = System.IO.Path.GetFileName(dlg.FileName);
                string destPath = System.IO.Path.Combine(folder, fileName);

                // Tránh trùng tên file
                int count = 1;
                while (File.Exists(destPath))
                {
                    string name = System.IO.Path.GetFileNameWithoutExtension(fileName);
                    string ext = System.IO.Path.GetExtension(fileName);
                    destPath = System.IO.Path.Combine(folder, $"{name}_{count}{ext}");
                    count++;
                }

                // Copy file vào thư mục HinhAnh
                File.Copy(dlg.FileName, destPath);

                // Lưu chỉ tên file (không lưu full path)
                _hinhAnh = System.IO.Path.GetFileName(destPath);
                imgPreview.Source = new BitmapImage(new Uri(destPath));
            }
        }


        private void BtnLuu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var nv = new NhanVien
                {
                    ID = _id ?? 0,
                    HoTen = txtHoTen.Text,
                    Email = txtEmail.Text,
                    SoDienThoai = txtSoDienThoai.Text,
                    VaiTro = cbVaiTro.Text,
                    TrangThai = chkTrangThai.IsChecked == true,
                    HinhAnh = _hinhAnh
                };

                // Xử lý mật khẩu
                if (_id == null) // THÊM MỚI
                {
                    if (string.IsNullOrWhiteSpace(txtMatKhau.Password))
                    {
                        MessageBox.Show("Mật khẩu không được để trống");
                        return;
                    }

                    if (txtMatKhau.Password.Length < 6 || txtMatKhau.Password.Length > 50)
                    {
                        MessageBox.Show("Mật khẩu phải từ 6 đến 50 ký tự");
                        return;
                    }

                    nv.MatKhau = PasswordHelper.HashPassword(txtMatKhau.Password);
                }
                else // SỬA
                {
                    // Lấy lại mật khẩu cũ từ database
                    var nvCu = _service.GetById(_id.Value);

                    if (nvCu == null)
                    {
                        MessageBox.Show("Không tìm thấy thông tin nhân viên!", "Lỗi");
                        return;
                    }

                    if (string.IsNullOrEmpty(nvCu.MatKhau))
                    {
                        MessageBox.Show("Mật khẩu cũ không hợp lệ. Vui lòng liên hệ admin!", "Lỗi");
                        return;
                    }

                    nv.MatKhau = nvCu.MatKhau;
                }

                // Validate
                if (!ValidateNhanVien(nv))
                    return;

                // Lưu
                bool result = _id == null
                    ? _service.Insert(nv)
                    : _service.Update(nv);

                if (result)
                {
                    DialogResult = true;
                    Close();
                }
                else
                {
                    MessageBox.Show("Lưu nhân viên thất bại", "Lỗi");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi");
            }
        }

        private void BtnHuy_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private bool ValidateNhanVien(NhanVien nv)
        {
            var context = new ValidationContext(nv);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            bool isValid = Validator.TryValidateObject(
                nv,
                context,
                results,
                true 
            );

            if (!isValid)
            {
                string errors = string.Join("\n", results.Select(x => "• " + x.ErrorMessage));
                MessageBox.Show(errors, "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            return isValid;
        }
    }
}

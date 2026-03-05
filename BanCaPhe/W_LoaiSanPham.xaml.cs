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

namespace BanCaPhe
{
  
    public partial class W_LoaiSanPham : UserControl
    {
        private readonly LoaiDoUongService _service = new LoaiDoUongService();
        public W_LoaiSanPham()
        {
            InitializeComponent();
            LoadDanhMuc();
        }

        private void LoadDanhMuc()
        {
            dgLoaiDoUong.ItemsSource = _service.GetAll();
            dgLoaiDoUong.SelectedIndex = -1;
            ClearForm();
        }

        private void ClearForm()
        {
            txtTenLoai.Text = "";
            txtViTri.Text = "";
        }


        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtTenLoai.Text))
            {
                MessageBox.Show("Tên loại không được để trống");
                return false;
            }

            if (!int.TryParse(txtViTri.Text, out _))
            {
                MessageBox.Show("Vị trí phải là số");
                return false;
            }

            return true;
        }



        private void dgLoaiDoUong_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgLoaiDoUong.SelectedItem is DanhMucDouong loai)
            {
                txtTenLoai.Text = loai.TenLoai;
                txtViTri.Text = loai.ViTri.ToString();
            }
        }

        private void BtnThem_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput()) return;

            int viTri = int.Parse(txtViTri.Text);

            if (_service.IsTenLoaiExists(txtTenLoai.Text.Trim(), viTri))
            {
                MessageBox.Show("Tên loại hoặc vị trí đã tồn tại");
                return;
            }

            var loai = new DanhMucDouong
            {
                TenLoai = txtTenLoai.Text.Trim(),
                ViTri = int.Parse(txtViTri.Text)
            };

            if (_service.Insert(loai))
            {
                MessageBox.Show("Thêm danh mục thành công");
                LoadDanhMuc();
            }
            else
            {
                MessageBox.Show("Thêm thất bại");
            }
        }


        private void BtnSua_Click(object sender, RoutedEventArgs e)
        {
            if (dgLoaiDoUong.SelectedItem is not DanhMucDouong selected)
            {
                MessageBox.Show("Vui lòng chọn danh mục cần sửa");
                return;
            }

            if (!ValidateInput()) return;

            selected.TenLoai = txtTenLoai.Text.Trim();
            selected.ViTri = int.Parse(txtViTri.Text);

            if (_service.Update(selected))
            {
                MessageBox.Show("Cập nhật thành công");
                LoadDanhMuc();
            }
            else
            {
                MessageBox.Show("Cập nhật thất bại");
            }
        }
        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (dgLoaiDoUong.SelectedItem is not DanhMucDouong selected)
            {
                MessageBox.Show("Vui lòng chọn danh mục cần xóa");
                return;
            }

            if (MessageBox.Show(
                $"Bạn có chắc muốn xóa '{selected.TenLoai}' ?",
                "Xác nhận",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning) != MessageBoxResult.Yes)
            {
                return;
            }

            if (_service.Delete(selected.ID))
            {
                MessageBox.Show("Xóa thành công");
                LoadDanhMuc();
            }
            else
            {
                MessageBox.Show("Xóa thất bại");
            }
        }
    }
}

using BanCaPhe.Models;
using BanCaPhe.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
   
    public partial class W_SanPhamModal : Window
    {
        private DoUongService _service = new DoUongService();
        private string _hinhAnh = "";
        private int _id = 0;

        public W_SanPhamModal()
        {
            InitializeComponent();
            LoadLoai();
        }

        public W_SanPhamModal(DoUong sp)
        {
            InitializeComponent();

            _id = sp.ID;
            LoadLoai();
            txtTen.Text = sp.TenDoUong;
            txtGia.Text = sp.Gia.ToString();
            chkConBan.IsChecked = sp.ConBan;
            txtMoTa.Text = sp.Mota;

            _hinhAnh = sp.HinhAnh;

            cbLoai.SelectedValue = sp.LoaiID;

            if (!string.IsNullOrEmpty(_hinhAnh))
            {
                string path = System.IO.Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "HinhAnh",
                    _hinhAnh
                );

                if (File.Exists(path))
                    imgPreview.Source = new BitmapImage(new Uri(path));
            }
        }


        void LoadLoai(int? selectedLoaiId = null)
        {
            var list = _service.GetLoai();
            cbLoai.ItemsSource = list;

            if (selectedLoaiId.HasValue)
            {
                // Tìm item có ID khớp
                var item = list.FirstOrDefault(x => x.ID == selectedLoaiId.Value);
                if (item != null)
                {
                    cbLoai.SelectedItem = item;
                }
            }
        }


        private void BtnChonHinh_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image (*.png;*.jpg)|*.png;*.jpg";

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

           
                int count = 1;
                while (File.Exists(destPath))
                {
                    string name = System.IO.Path.GetFileNameWithoutExtension(fileName);
                    string ext = System.IO.Path.GetExtension(fileName);
                    destPath = System.IO.Path.Combine(folder, $"{name}_{count}{ext}");
                    count++;
                }

                File.Copy(dlg.FileName, destPath);

                _hinhAnh = System.IO.Path.GetFileName(destPath);
                imgPreview.Source = new BitmapImage(new Uri(destPath));

            }
        }

        private void BtnLuu_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTen.Text))
            {
                MessageBox.Show("Tên đồ uống không được để trống");
                return;
            }

            if (!decimal.TryParse(txtGia.Text, out decimal gia))
            {
                MessageBox.Show("Giá không hợp lệ");
                return;
            }

            if (cbLoai.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn loại đồ uống");
                return;
            }

            if (string.IsNullOrEmpty(_hinhAnh))
            {
                MessageBox.Show("Vui lòng chọn hình ảnh");
                return;
            }

            var sp = new DoUong
            {
                ID = _id,
                TenDoUong = txtTen.Text,
                Gia = decimal.Parse(txtGia.Text),
                LoaiID = (int)cbLoai.SelectedValue,
                Mota = txtMoTa.Text,
                ConBan = chkConBan.IsChecked == true,
                HinhAnh = _hinhAnh
            };

            if (_id == 0)
                _service.Insert(sp);
            else
                _service.Update(sp);

            DialogResult = true;
        }

        private void BtnHuy_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

using BanCaPhe.Helpers;
using BanCaPhe.Models;
using BanCaPhe.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// <summary>
    /// Interaction logic for W_ToppingModal.xaml
    /// </summary>
    public partial class W_ToppingModal : Window
    {

        private readonly ToppingService _service;
        private int? _id = null;
        private string _hinhAnh = null;
        public W_ToppingModal()
        {
            InitializeComponent();
            _service = new ToppingService();
        }

        public W_ToppingModal(int id) : this()
        {
            _id = id;
            LoadTopping();
        }

        private void LoadTopping()
        {
            var tp = _service.GetById(_id.Value);
            if (tp == null) return;

            txtTen.Text = tp.TenTopping;
            txtGia.Text = tp.Gia.ToString(CultureInfo.InvariantCulture);
            chkBanRieng.IsChecked = tp.CoTheBanRieng;
            chkConBan.IsChecked = tp.ConBan;
            _hinhAnh = tp.HinhAnh;

            if (!string.IsNullOrEmpty(_hinhAnh) && File.Exists(_hinhAnh))
            {
                imgPreview.Source = new BitmapImage(new Uri(_hinhAnh));
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
                _hinhAnh = dlg.FileName;
                imgPreview.Source = new BitmapImage(new Uri(_hinhAnh));
            }
        }

        private void BtnLuu_Click(object sender, RoutedEventArgs e)
        {
            var topping = new Topping
            {
                ID = _id ?? 0,
                TenTopping = txtTen.Text.Trim(),
                Gia = decimal.TryParse(txtGia.Text, out decimal g) ? g : -1,
                CoTheBanRieng = chkBanRieng.IsChecked == true,
                ConBan = chkConBan.IsChecked == true,
                HinhAnh = _hinhAnh
            };

            if (!ValidationHelper.ValidateWithReport(topping)) return;

            bool result = _id == null
                ? _service.Insert(topping)
                : _service.Update(topping);

            if (result)
            {
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Lưu topping thất bại");
            }
        }

        private void BtnHuy_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    
}
}

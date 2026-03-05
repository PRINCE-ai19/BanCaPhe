using BanCaPhe.Models;
using BanCaPhe.Services;
using BanCaPhe.Views;
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

    public partial class UC_SanPham : UserControl
    {
        private DoUongService _service = new DoUongService();

        public UC_SanPham()
        {
            InitializeComponent();
            LoadData();
        }

        void LoadData()
        {
            dgSanPham.ItemsSource = _service.GetAll();
        }

        private void BtnThem_Click(object sender, RoutedEventArgs e)
        {
            var w = new W_SanPhamModal();
            if (w.ShowDialog() == true)
                LoadData();
        }

        private void BtnSua_Click(object sender, RoutedEventArgs e)
        {
            if (dgSanPham.SelectedItem is DoUong sp)
            {
                var w = new W_SanPhamModal(sp);
                if (w.ShowDialog() == true)
                    LoadData();
            }
        }

        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (dgSanPham.SelectedItem is DoUong sp)
            {
                _service.Delete(sp.ID);
                LoadData();
            }
        }
    }

}

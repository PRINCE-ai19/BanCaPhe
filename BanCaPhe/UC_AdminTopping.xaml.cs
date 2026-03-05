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
    /// <summary>
    /// Interaction logic for UC_AdminTopping.xaml
    /// </summary>
    public partial class UC_AdminTopping : UserControl
    {

        private readonly ToppingService _service;

        public UC_AdminTopping()
        {
            InitializeComponent();
            _service = new ToppingService();
            LoadData();
        }

        private void LoadData()
        {
            List<Topping> list = _service.GetAll();
            dgTopping.ItemsSource = list;
        }

        private void BtnThem_Click(object sender, RoutedEventArgs e)
        {
            var modal = new W_ToppingModal(); // thêm
            if (modal.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void BtnSua_Click(object sender, RoutedEventArgs e)
        {
            if (dgTopping.SelectedItem is not Topping selected)
            {
                MessageBox.Show("Vui lòng chọn topping cần sửa");
                return;
            }

            var modal = new W_ToppingModal(selected.ID);
            if (modal.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (dgTopping.SelectedItem is not Topping selected)
            {
                MessageBox.Show("Vui lòng chọn topping cần xóa");
                return;
            }

            var confirm = MessageBox.Show(
                $"Bạn có chắc muốn xóa topping \"{selected.TenTopping}\"?",
                "Xác nhận",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (confirm != MessageBoxResult.Yes)
                return;

            bool result = _service.Delete(selected.ID);

            if (result)
            {
                MessageBox.Show("Xóa thành công");
                LoadData();
            }
            else
            {
                MessageBox.Show("Xóa thất bại");
            }
        }
    }
}

using BanCaPhe.Models;
using BanCaPhe.ViewModel;
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
    /// Interaction logic for ProductDetailWindow.xaml
    /// </summary>
    public partial class ProductDetailWindow : Window
    {
        public ProductDetailWindow()
        {
            InitializeComponent();
          
        }

        private void ThemVaoGio_Click(object sender, RoutedEventArgs e)
        {
            // Đóng cửa sổ sau khi thêm vào giỏ thành công
            DialogResult = true;
            Close();
        }
    }
}

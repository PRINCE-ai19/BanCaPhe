using BanCaPhe.Models;
using BanCaPhe.Services;
using BanCaPhe.ViewModel;
using BanCaPhe.Views;
using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
      

        public MainWindow()
        {
            InitializeComponent();
         
            DataContext = new MainViewModel();

         
        }

        private void Topping_Click(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is MainViewModel vm)
            {
                vm.HienThiTopping();
            }
        }

        private void SanPham_Click(object sender, MouseButtonEventArgs e)
        {
            var sp = (sender as Border)?.DataContext as DoUong;
            if (sp == null) return;

            var sizeService = new KichThuocService();
            var sizes = sizeService.GetBySanPhamId(sp.ID);

            var vm = new ProductDetailViewModel(sp, sizes);
            var window = new ProductDetailWindow
            {
                DataContext = vm,
                Owner = this
            };

            window.ShowDialog();
        }

        private void Toppingmua_Click(object sender, MouseButtonEventArgs e)
        {

            var border = sender as Border;
            if (border == null) return;

            var topping = border.DataContext as Topping;
            if (topping == null) return;

            // Tạo ViewModel với topping
            var vm = new ToppingDetailViewModel(topping);

            // Mở cửa sổ và set DataContext
            var window = new ToppingDetailWindow
            {
                DataContext = vm,  // ← QUAN TRỌNG
                Owner = this
            };
            window.ShowDialog();
        }


        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is MainViewModel vm && vm.MoGhiChuCommand.CanExecute(null))
            {
                vm.MoGhiChuCommand.Execute(null);
            }
        }


        private void ListViewItem_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item)
            {
                item.IsSelected = true;

                if (item.DataContext is OrderItem orderItem)
                {
                  
                    if (string.IsNullOrEmpty(orderItem.Size))
                        return;
                }

                if (DataContext is MainViewModel vm &&
                    vm.MoGhiChuCommand.CanExecute(null))
                {
                    vm.MoGhiChuCommand.Execute(null);
                }
            }
        }

        private void BlockDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true; //  chặn không cho nổi lên ListViewItem
        }

        private void Logout_MouseEnter(object sender, MouseEventArgs e)
        {
            LogoutBorder.Background = new SolidColorBrush(Color.FromRgb(211, 47, 47)); // đỏ đậm
        }

        private void Logout_MouseLeave(object sender, MouseEventArgs e)
        {
            LogoutBorder.Background = new SolidColorBrush(Color.FromRgb(255, 245, 245)); // đỏ nhạt
        }

        private void Logout_Click(object sender, MouseButtonEventArgs e)
        {
            W_DangNhap login = new W_DangNhap();
            login.Show();
            this.Close();
        }
    }
}
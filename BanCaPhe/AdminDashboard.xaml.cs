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

namespace BanCaPhe
{

    public partial class AdminDashboard : Window
    {
        private AdminDashboardViewModel ViewModel => DataContext as AdminDashboardViewModel;

        public AdminDashboard()
        {
            InitializeComponent();

       
            AdimMainContent.Loaded += (s, e) => RefreshDashboardData();
        }

        private void RefreshDashboardData()
        {
            ViewModel?.LoadAllData();
        }

        private void DoanhThu_Click(object sender, MouseButtonEventArgs e)
        {
            AdimMainContent.Content = new UC_DoanhThu();
        }

        private void SanPham_Click(object sender, MouseButtonEventArgs e)
        {
            AdimMainContent.Content = new UC_SanPham();
            // Refresh khi quay lại
            RefreshDashboardData();
        }

        private void LoaiSanPham_Click(object sender, MouseButtonEventArgs e)
        {
            AdimMainContent.Content = new W_LoaiSanPham();
            // Refresh khi quay lại
            RefreshDashboardData();
        }

        private void Topping_Click(object sender, MouseButtonEventArgs e)
        {
            AdimMainContent.Content = new UC_AdminTopping();
            // Refresh khi quay lại
            RefreshDashboardData();
        }

        private void NhanVien_Click(object sender, MouseButtonEventArgs e)
        {
            AdimMainContent.Content = new UC_NhanVien();
            // Refresh khi quay lại
            RefreshDashboardData();
        }

        private void Logout_MouseEnter(object sender, MouseEventArgs e)
        {
            LogoutCard.Background = new SolidColorBrush(Color.FromRgb(255, 77, 79));
        }

        private void Logout_MouseLeave(object sender, MouseEventArgs e)
        {
            LogoutCard.Background = new SolidColorBrush(Color.FromRgb(255, 245, 245));
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            W_DangNhap login = new W_DangNhap();
            login.Show();
            this.Close();
        }

        /// <summary>
        /// Override OnActivated để refresh khi window được activate lại
        /// (Hữu ích khi đóng dialog và quay lại)
        /// </summary>
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            RefreshDashboardData();
        }
    }



}

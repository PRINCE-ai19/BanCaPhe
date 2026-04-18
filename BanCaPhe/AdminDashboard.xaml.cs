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
        }

 
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            ViewModel?.LoadAllData();
        }
    }
}

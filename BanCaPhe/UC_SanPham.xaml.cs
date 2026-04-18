using BanCaPhe.ViewModel;
using System.Windows.Controls;

namespace BanCaPhe
{
    public partial class UC_SanPham : UserControl
    {
        public UC_SanPham()
        {
            InitializeComponent();
            this.DataContext = new SanPhamViewModel();
        }
    }
}

using BanCaPhe.Helpers;
using BanCaPhe.Models;
using BanCaPhe.Services;
using BanCaPhe.ViewModel;
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
        public W_SanPhamModal()
        {
            InitializeComponent();
            DataContext = new SanPhamModalViewModel();
        }

        public W_SanPhamModal(DoUong sp)
        {
            InitializeComponent();
            DataContext = new SanPhamModalViewModel(sp);
        }
    }
}

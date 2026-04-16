using BanCaPhe.Helpers;
using BanCaPhe.Models;
using BanCaPhe.Services;
using BanCaPhe.ViewModel;
using Microsoft.Identity.Client;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public partial class W_NhanVienModal : Window
    {
        public W_NhanVienModal()
        {
            InitializeComponent();
            DataContext = new NhanVienModalViewModel();
        }

        public W_NhanVienModal(int id)
        {
            InitializeComponent();
            DataContext = new NhanVienModalViewModel(id);
        }
    }
}

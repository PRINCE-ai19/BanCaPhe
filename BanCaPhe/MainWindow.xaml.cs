using BanCaPhe.ViewModel;
using System.Windows;

namespace BanCaPhe
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}
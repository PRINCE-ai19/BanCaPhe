using System.Windows;

namespace BanCaPhe
{
    public partial class VoucherModal : Window
    {
        public VoucherModal()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}

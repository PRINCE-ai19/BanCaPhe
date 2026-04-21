using System.Windows;

namespace BanCaPhe.Views
{
    public partial class XacNhanVoucherWindow : Window
    {
        public XacNhanVoucherWindow()
        {
            InitializeComponent();
        }

        public XacNhanVoucherWindow(string maCode, string tenChuongTrinh, string giaTriGiam, 
            decimal tongTienGoc, decimal soTienGiam, decimal tongThanhToan) : this()
        {
            txtMaCode.Text = maCode;
            txtTenChuongTrinh.Text = tenChuongTrinh;
            txtGiaTriGiam.Text = giaTriGiam;
            txtTongTienGoc.Text = $"{tongTienGoc:N0} đ";
            txtSoTienGiam.Text = $"- {soTienGiam:N0} đ";
            txtTongThanhToan.Text = $"{tongThanhToan:N0} đ";
        }

        private void BtnXacNhan_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void BtnHuy_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}

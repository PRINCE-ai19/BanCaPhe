using BanCaPhe.Models;
using BanCaPhe.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BanCaPhe.ViewModel
{
    internal class ThanhToanViewModel : BaseViewModel
    {
        public decimal TongTien { get; }
        public ObservableCollection<OrderItem> Items { get; }

        public ICommand TienMatCommand { get; }
        public ICommand ChuyenKhoanCommand { get; }
        public ICommand VnpayCommand { get; }
        public ICommand DongCommand { get; }

        public ThanhToanViewModel(decimal tongTien, ObservableCollection<OrderItem> items)
        {
            TongTien = tongTien;
            Items = items;

            TienMatCommand = new RelayCommand(_ => ThanhToanTienMat());
            ChuyenKhoanCommand = new RelayCommand(_ => ThanhToanChuyenKhoan());
            VnpayCommand = new RelayCommand(_ => ThanhToanVnpay());
            DongCommand = new RelayCommand(_ => Dong());
        }

        //  TIỀN MẶT 
        private void ThanhToanTienMat()
        {
            // mở view thanh toán tiền mặt
            var vm = new ThanhToanTienMatViewModel(TongTien);

            var view = new ThanhToanTienMatView
            {
                DataContext = vm,
                Owner = Application.Current.Windows
                    .OfType<Window>()
                    .FirstOrDefault(w => w.IsActive)
            };

            view.ShowDialog();

            // đóng popup chọn phương thức
            Dong();
        }

        //  CHUYỂN KHOẢN 
        private void ThanhToanChuyenKhoan()
        {
            DialogService.ShowMessage("Thanh toán chuyển khoản (sẽ làm sau)");
        }

        // VNPAY
        private void ThanhToanVnpay()
        {
            DialogService.ShowMessage("Thanh toán VNPay QR (sẽ tích hợp sau)");
        }

        private void Dong()
        {
            Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w.IsActive)
                ?.Close();
        }
    }
}

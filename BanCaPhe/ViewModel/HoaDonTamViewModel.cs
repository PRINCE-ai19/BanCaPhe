using BanCaPhe.Models;
using BanCaPhe.Pdf;
using Microsoft.Win32;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BanCaPhe.ViewModel
{
    internal class HoaDonTamViewModel : BaseViewModel
    {
        public ObservableCollection<OrderItem> Items { get; }
        public decimal TongTien { get; }

        public ICommand XuatPdfCommand { get; }

        public HoaDonTamViewModel(
            ObservableCollection<OrderItem> items,
            decimal tongTien)
        {
            Items = new ObservableCollection<OrderItem>(items);
            TongTien = tongTien;

            XuatPdfCommand = new RelayCommand(_ => XuatPdf());
        }

        private void XuatPdf()
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var saveDialog = new SaveFileDialog
            {
                Filter = "PDF file (*.pdf)|*.pdf",
                FileName = $"HoaDonTam_{DateTime.Now:yyyyMMdd_HHmm}.pdf"
            };

            if (saveDialog.ShowDialog() == true)
            {
                var document = new HoaDonTamPdf(Items, TongTien);
                document.GeneratePdf(saveDialog.FileName);
            }
        }
    }
}

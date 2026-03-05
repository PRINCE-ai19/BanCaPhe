using BanCaPhe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BanCaPhe.ViewModel
{
    class GhiChuViewModel : BaseViewModel
    {

        public OrderItem Item { get; }

        public string Size
        {
            get => Item.Size;
            set { Item.Size = value; OnPropertyChanged(); }
        }

        public string GhiChu
        {
            get => Item.GhiChu;
            set { Item.GhiChu = value; OnPropertyChanged(); }
        }

        public ICommand ChonNhanhCommand { get; }
        public ICommand DongCommand { get; }

        public GhiChuViewModel(OrderItem item)
        {
            Item = item;

            ChonNhanhCommand = new RelayCommand(p =>
            {
                if (p == null) return;
                GhiChu = string.IsNullOrEmpty(GhiChu)
                    ? p.ToString()
                    : GhiChu + ", " + p;
            });

            DongCommand = new RelayCommand(w =>
            {
                (w as Window)?.Close();
            });
        }
    }
}

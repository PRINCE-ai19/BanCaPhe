using BanCaPhe.Models;
using BanCaPhe.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanCaPhe.Services
{
    public class CartService : BaseViewModel
    {

        private static CartService _instance;
        public static CartService Instance => _instance ??= new CartService();

        public ObservableCollection<OrderItem> Items { get; }
            = new ObservableCollection<OrderItem>();

        public decimal TongTien =>
            Items.Sum(i => i.TongTien);

        public void Clear()
        {
            Items.Clear();
        }

        public void NotifyTongTienChanged()
        {
            OnPropertyChanged(nameof(TongTien));
        }
    }
}

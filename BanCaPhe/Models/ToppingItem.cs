using BanCaPhe.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanCaPhe.Models
{
    public class ToppingItem :BaseViewModel
    {
        public int ToppingID { get; set; }
        public string Ten { get; set; } = "";

        private int _soLuong = 1;
        public int SoLuong
        {
            get => _soLuong;
            set
            {
                if (value < 1) value = 1;
                _soLuong = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ThanhTien));
            }
        }

        public decimal Gia { get; set; }

        public decimal ThanhTien => Gia * SoLuong;


    }
}

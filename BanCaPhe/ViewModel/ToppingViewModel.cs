using BanCaPhe.Models;
using BanCaPhe.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BanCaPhe.ViewModel
{
    public class ToppingViewModel : BaseViewModel
    {
        private readonly ToppingService _service;
        private ObservableCollection<Topping> _danhSachTopping;
        public ObservableCollection<Topping> DanhSachTopping
        {
            get => _danhSachTopping;
            set
            {
                _danhSachTopping = value;
                OnPropertyChanged();
            }
        }

        private Topping _selectedTopping;
        public Topping SelectedTopping
        {
            get => _selectedTopping;
            set
            {
                _selectedTopping = value;
                OnPropertyChanged();
            }
        }

        public ToppingViewModel()
        {
            _service = new ToppingService();
            LoadData();
        }

        private void LoadData()
        {
            var data = _service.GetAll();
            DanhSachTopping = new ObservableCollection<Topping>(data);
        }

    }
}

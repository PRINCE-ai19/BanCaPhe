using BanCaPhe.Services;
using BanCaPhe.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BanCaPhe.Models
{

    public class OrderItem : BaseViewModel
    {
        public bool LaTraSua { get; set; }
        public string Ten { get; set; } = "";
        public decimal DonGia { get; set; }
        public int? SanPhamKichThuocID { get; set; }

        private string _size;
        public string Size
        {
            get => _size;
            set
            {
                _size = value;
                OnPropertyChanged();
            }
        }

        private int _soLuong = 1;
        public int SoLuong
        {
            get => _soLuong;
            set
            {
                if (value < 0) value = 0;
                _soLuong = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ThanhTien));
                OnPropertyChanged(nameof(TongTien));
                OnPropertyChanged(nameof(CoTheThemTopping));

                if (_soLuong == 0)
                    Toppings.Clear();
            }
        }

        public decimal ThanhTien => DonGia * SoLuong;

        private string _ghiChu;
        public string GhiChu
        {
            get => _ghiChu;
            set
            {
                _ghiChu = value;
                OnPropertyChanged();
            }
        }

        // TOPPING
        private ObservableCollection<ToppingItem> _toppings;
        public ObservableCollection<ToppingItem> Toppings
        {
            get => _toppings;
            set
            {
                if (_toppings != null)
                {
                    _toppings.CollectionChanged -= Toppings_CollectionChanged;
                }

                _toppings = value;

                if (_toppings != null)
                {
                    _toppings.CollectionChanged += Toppings_CollectionChanged;
                }

                OnPropertyChanged();
                OnPropertyChanged(nameof(TongTien));
            }
        }

        public OrderItem()
        {
            _toppings = new ObservableCollection<ToppingItem>();
            _toppings.CollectionChanged += Toppings_CollectionChanged;
        }

        private void Toppings_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(TongTien));

            // Cập nhật khi số lượng topping thay đổi
            if (e.NewItems != null)
            {
                foreach (ToppingItem item in e.NewItems)
                {
                    item.PropertyChanged += ToppingItem_PropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (ToppingItem item in e.OldItems)
                {
                    item.PropertyChanged -= ToppingItem_PropertyChanged;
                }
            }
        }

        private void ToppingItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ToppingItem.SoLuong) || e.PropertyName == nameof(ToppingItem.ThanhTien))
            {
                OnPropertyChanged(nameof(TongTien));
            }
        }

        public bool CoTheThemTopping => SoLuong > 0 && LaTraSua;

        public void ThemTopping(ToppingItem topping)
        {
            if (!CoTheThemTopping)
                throw new InvalidOperationException("Chỉ trà sữa mới được thêm topping");

            Toppings.Add(topping);
            OnPropertyChanged(nameof(TongTien));
        }

        public decimal TongTien
        {
            get
            {
                decimal tien = ThanhTien;
                if (Toppings != null)
                {
                    foreach (var t in Toppings)
                    {
                        tien += t.ThanhTien;
                    }
                }
                return tien;
            }
        }

        public ICommand TangCommand => new RelayCommand(_ => SoLuong++);
        public ICommand GiamCommand => new RelayCommand(_ => SoLuong--);
        public ICommand XoaCommand => new RelayCommand(_ =>
        {
            CartService.Instance.Items.Remove(this);
        });
    }



}

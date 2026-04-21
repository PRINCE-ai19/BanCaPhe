using BanCaPhe.Helpers;
using BanCaPhe.Models;
using BanCaPhe.Services;
using BanCaPhe.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace BanCaPhe.ViewModel
{
    public class UC_KhachHangViewModel : BaseViewModel
    {
        private readonly KhachHangService _khachHangService;

        private ObservableCollection<KhachHang> _danhSachKhachHang;
        public ObservableCollection<KhachHang> DanhSachKhachHang
        {
            get => _danhSachKhachHang;
            set { _danhSachKhachHang = value; OnPropertyChanged(); }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set 
            { 
                _searchText = value; 
                OnPropertyChanged();
                Search();
            }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand RefreshCommand { get; }

        public UC_KhachHangViewModel()
        {
            _khachHangService = new KhachHangService();
            
            AddCommand = new RelayCommand(_ => ExecuteAdd());
            EditCommand = new RelayCommand<KhachHang>(ExecuteEdit);
            DeleteCommand = new RelayCommand<KhachHang>(ExecuteDelete);
            RefreshCommand = new RelayCommand(_ => LoadData());

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var list = _khachHangService.SearchByPhone("");
                DanhSachKhachHang = new ObservableCollection<KhachHang>(list);
            }
            catch (Exception ex)
            {
                DialogService.ShowError("Lỗi tải danh sách: " + ex.Message);
            }
        }

        private void Search()
        {
            try
            {
                var list = _khachHangService.SearchByPhone(SearchText ?? "");
                DanhSachKhachHang = new ObservableCollection<KhachHang>(list);
            }
            catch { }
        }

        private void ExecuteAdd()
        {
            var vm = new W_KhachHangModalViewModel();
            var view = new W_KhachHangModal { DataContext = vm };
            if (view.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void ExecuteEdit(KhachHang kh)
        {
            if (kh == null) return;
            var vm = new W_KhachHangModalViewModel(kh);
            var view = new W_KhachHangModal { DataContext = vm };
            if (view.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void ExecuteDelete(KhachHang kh)
        {
            if (kh == null) return;
            if (DialogService.ShowConfirm($"Bạn có chắc muốn xóa khách hàng {kh.HoTen}?"))
            {
                try
                {
                    _khachHangService.Delete(kh.ID);
                    DialogService.ShowMessage("Đã xóa khách hàng thành công.");
                    LoadData();
                }
                catch (Exception ex)
                {
                    DialogService.ShowError("Lỗi xóa: " + ex.Message);
                }
            }
        }
    }
}

using BanCaPhe.Helpers;
using BanCaPhe.Models;
using BanCaPhe.Services;
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
    public class ToppingViewModel : BaseViewModel
    {
        private readonly ToppingService _service;
        private ObservableCollection<Topping> _danhSachTopping = new();
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

        public ICommand ThemCommand { get; }
        public ICommand SuaCommand { get; }
        public ICommand XoaCommand { get; }

        public ToppingViewModel()
        {
            _service = new ToppingService();
            
            ThemCommand = new RelayCommand(ExecuteThem);
            SuaCommand = new RelayCommand(ExecuteSua);
            XoaCommand = new RelayCommand(ExecuteXoa);

            LoadData();
        }

        public void LoadData()
        {
            try
            {
                var data = _service.GetAllAdmin();
                if (data == null)
                {
                    DanhSachTopping = new ObservableCollection<Topping>();
                }
                else
                {
                    DanhSachTopping = new ObservableCollection<Topping>(data);
                }
            }
            catch (Exception ex)
            {
                DialogService.ShowError("Lỗi khi tải danh sách Topping: " + ex.Message);
                DanhSachTopping = new ObservableCollection<Topping>();
            }
        }

        private void ExecuteThem(object obj)
        {
            var win = new W_ToppingModal();
            // Đẩy logic gán DataContext lên ViewModel xử lý
            win.DataContext = new ToppingDetailViewModel(new Topping());
            
            if (win.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void ExecuteSua(object obj)
        {
            var topping = obj as Topping ?? SelectedTopping;
            if (topping == null)
            {
                DialogService.ShowError("Vui lòng chọn Topping cần sửa");
                return;
            }

            var win = new W_ToppingModal();
            // Lấy dữ liệu từ Service rồi mới gán vào ViewModel cho Modal
            var toppingFromDb = _service.GetById(topping.ID);
            if (toppingFromDb == null)
            {
                DialogService.ShowError("Không tìm thấy dữ liệu Topping này!");
                return;
            }
            win.DataContext = new ToppingDetailViewModel(toppingFromDb);

            if (win.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void ExecuteXoa(object obj)
        {
            var topping = obj as Topping ?? SelectedTopping;
            if (topping == null)
            {
                DialogService.ShowError("Vui lòng chọn Topping cần xóa");
                return;
            }

            if (DialogService.ShowConfirm($"Bạn có chắc muốn xóa (ngừng bán) Topping '{topping.TenTopping}'?"))
            {
                try
                {
                    if (_service.Delete(topping.ID))
                    {
                        LoadData();
                        DialogService.ShowMessage("Xóa Topping thành công!");
                    }
                    else
                    {
                        DialogService.ShowError("Xóa Topping thất bại!");
                    }
                }
                catch (Exception ex)
                {
                    // Bắt lỗi RAISERROR từ SQL (ví dụ: Topping này hiện không còn bán!)
                    DialogService.ShowError(ex.Message);
                }
            }
        }
    }
}

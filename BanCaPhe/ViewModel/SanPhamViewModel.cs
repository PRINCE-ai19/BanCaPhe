using BanCaPhe.Helpers;
using BanCaPhe.Models;
using BanCaPhe.Services;
using BanCaPhe.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BanCaPhe.ViewModel
{
    public class SanPhamViewModel : BaseViewModel
    {
        private readonly DoUongService _service = new DoUongService();

        private ObservableCollection<DoUong> _sanPhamList;
        public ObservableCollection<DoUong> SanPhamList
        {
            get => _sanPhamList;
            set { _sanPhamList = value; OnPropertyChanged(); }
        }

        private DoUong _selectedSanPham;
        public DoUong SelectedSanPham
        {
            get => _selectedSanPham;
            set { _selectedSanPham = value; OnPropertyChanged(); }
        }

        public ICommand ThemCommand { get; }
        public ICommand SuaCommand { get; }
        public ICommand XoaCommand { get; }
        public ICommand RefreshCommand { get; }

        public SanPhamViewModel()
        {
            ThemCommand = new RelayCommand(ExecuteThem);
            SuaCommand = new RelayCommand(ExecuteSua);
            XoaCommand = new RelayCommand(ExecuteXoa);
            RefreshCommand = new RelayCommand(_ => LoadData());
            LoadData();
        }

        public void LoadData()
        {
            try
            {
                var list = _service.GetAllAdmin();
                SanPhamList = new ObservableCollection<DoUong>(list);
            }
            catch (Exception ex)
            {
                DialogService.ShowError("Lỗi khi tải danh sách: " + ex.Message);
            }
        }

        private void ExecuteThem(object obj)
        {
            var w = new W_SanPhamModal();
            if (w.ShowDialog() == true)
                LoadData();
        }

        private void ExecuteSua(object obj)
        {
            if (SelectedSanPham == null)
            {
                DialogService.ShowError("Vui lòng chọn sản phẩm cần sửa");
                return;
            }

            var w = new W_SanPhamModal(SelectedSanPham);
            if (w.ShowDialog() == true)
                LoadData();
        }

        private void ExecuteXoa(object obj)
        {
            if (SelectedSanPham == null)
            {
                DialogService.ShowError("Vui lòng chọn sản phẩm cần ngừng bán");
                return;
            }

            if (DialogService.ShowConfirm($"Bạn có chắc muốn ngừng bán sản phẩm '{SelectedSanPham.TenDoUong}'?"))
            {
                try
                {
                    if (_service.Delete(SelectedSanPham.ID))
                    {
                        DialogService.ShowMessage("Sản phẩm đã được chuyển sang trạng thái ngừng bán.");
                        LoadData();
                    }
                    else
                    {
                        DialogService.ShowError("Thao tác thất bại.");
                    }
                }
                catch (SqlException ex)
                {
                    DialogService.ShowError(ex.Message);
                }
                catch (Exception ex)
                {
                    DialogService.ShowError("Có lỗi xảy ra: " + ex.Message);
                }
            }
        }
    }
}

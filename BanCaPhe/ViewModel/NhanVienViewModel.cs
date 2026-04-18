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
    public class NhanVienViewModel : BaseViewModel
    {
        private readonly NhanVienService _service = new NhanVienService();

        private ObservableCollection<NhanVien> _listNhanVien;
        public ObservableCollection<NhanVien> ListNhanVien
        {
            get => _listNhanVien;
            set { _listNhanVien = value; OnPropertyChanged(); }
        }

        private NhanVien _selectedNhanVien;
        public NhanVien SelectedNhanVien
        {
            get => _selectedNhanVien;
            set { _selectedNhanVien = value; OnPropertyChanged(); }
        }

        public ICommand ThemCommand { get; }
        public ICommand SuaCommand { get; }
        public ICommand XoaCommand { get; }
        public ICommand DoiMatKhauCommand { get; }

        public NhanVienViewModel()
        {
            ThemCommand = new RelayCommand(ExecuteThem);
            SuaCommand = new RelayCommand(ExecuteSua);
            XoaCommand = new RelayCommand(ExecuteXoa);
            DoiMatKhauCommand = new RelayCommand(ExecuteDoiMatKhau);

            LoadData();
        }

        public void LoadData()
        {
            var list = _service.GetAll();
            ListNhanVien = new ObservableCollection<NhanVien>(list);
        }

        private void ExecuteThem(object obj)
        {
            var win = new W_NhanVienModal();
            win.DataContext = new NhanVienModalViewModel();
            if (win.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void ExecuteSua(object obj)
        {
            if (SelectedNhanVien == null)
            {
                DialogService.ShowError("Vui lòng chọn nhân viên cần sửa");
                return;
            }

            var win = new W_NhanVienModal();
            win.DataContext = new NhanVienModalViewModel(SelectedNhanVien.ID);
            if (win.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void ExecuteXoa(object obj)
        {
            if (SelectedNhanVien == null)
            {
                DialogService.ShowError("Vui lòng chọn nhân viên cần xóa");
                return;
            }

            if (DialogService.ShowConfirm($"Bạn có chắc muốn xóa nhân viên '{SelectedNhanVien.HoTen}'?"))
            {
                try
                {
                    if (_service.Delete(SelectedNhanVien.ID))
                    {
                        LoadData();
                        DialogService.ShowMessage("Xóa nhân viên thành công!");
                    }
                    else
                    {
                        DialogService.ShowError("Xóa nhân viên thất bại!");
                    }
                }
                catch (Exception ex)
                {
                    DialogService.ShowError(ex.Message);
                }
            }
        }

        private void ExecuteDoiMatKhau(object obj)
        {
            if (SelectedNhanVien == null)
            {
                DialogService.ShowError("Vui lòng chọn nhân viên cần đổi mật khẩu");
                return;
            }

            var dialog = new W_DoiMatKhauModal();
            dialog.DataContext = new DoiMatKhauViewModel(SelectedNhanVien.ID, SelectedNhanVien.HoTen);
            dialog.ShowDialog();
        }
    }
}

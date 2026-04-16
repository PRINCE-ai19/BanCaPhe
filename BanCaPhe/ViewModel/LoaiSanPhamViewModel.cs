using BanCaPhe.Helpers;
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
    public class LoaiSanPhamViewModel : BaseViewModel
    {
        private readonly LoaiDoUongService _service = new LoaiDoUongService();

        private ObservableCollection<DanhMucDouong> _danhMucList;
        public ObservableCollection<DanhMucDouong> DanhMucList
        {
            get => _danhMucList;
            set { _danhMucList = value; OnPropertyChanged(); }
        }

        private DanhMucDouong _selectedDanhMuc;
        public DanhMucDouong SelectedDanhMuc
        {
            get => _selectedDanhMuc;
            set 
            { 
                _selectedDanhMuc = value; 
                OnPropertyChanged();
                UpdateFormFromSelection();
            }
        }

        private string _tenLoai;
        public string TenLoai
        {
            get => _tenLoai;
            set { _tenLoai = value; OnPropertyChanged(); }
        }

        private string _viTri;
        public string ViTri
        {
            get => _viTri;
            set { _viTri = value; OnPropertyChanged(); }
        }

        public ICommand ThemCommand { get; }
        public ICommand SuaCommand { get; }
        public ICommand XoaCommand { get; }

        public LoaiSanPhamViewModel()
        {
            ThemCommand = new RelayCommand(ExecuteThem);
            SuaCommand = new RelayCommand(ExecuteSua);
            XoaCommand = new RelayCommand(ExecuteXoa);
            LoadDanhMuc();
        }

        private void LoadDanhMuc()
        {
            var list = _service.GetAll();
            DanhMucList = new ObservableCollection<DanhMucDouong>(list);
            ClearForm();
        }

        private void ClearForm()
        {
            TenLoai = "";
            ViTri = "";
            SelectedDanhMuc = null;
        }

        private void UpdateFormFromSelection()
        {
            if (SelectedDanhMuc != null)
            {
                TenLoai = SelectedDanhMuc.TenLoai;
                ViTri = SelectedDanhMuc.ViTri.ToString();
            }
        }

        private void ExecuteThem(object obj)
        {
            var loai = new DanhMucDouong
            {
                TenLoai = TenLoai?.Trim(),
                ViTri = int.TryParse(ViTri, out int v) ? v : 0
            };

            if (!ValidationHelper.ValidateWithReport(loai)) return;

            if (_service.IsTenLoaiExists(loai.TenLoai, loai.ViTri))
            {
                DialogService.ShowError("Tên loại hoặc vị trí đã tồn tại");
                return;
            }

            if (_service.Insert(loai))
            {
                DialogService.ShowMessage("Thêm danh mục thành công");
                LoadDanhMuc();
            }
            else
            {
                DialogService.ShowError("Thêm thất bại");
            }
        }

        private void ExecuteSua(object obj)
        {
            if (SelectedDanhMuc == null)
            {
                DialogService.ShowError("Vui lòng chọn danh mục cần sửa");
                return;
            }

            SelectedDanhMuc.TenLoai = TenLoai?.Trim();
            SelectedDanhMuc.ViTri = int.TryParse(ViTri, out int v) ? v : 0;

            if (!ValidationHelper.ValidateWithReport(SelectedDanhMuc)) return;

            if (_service.Update(SelectedDanhMuc))
            {
                DialogService.ShowMessage("Cập nhật thành công");
                LoadDanhMuc();
            }
            else
            {
                DialogService.ShowError("Cập nhật thất bại");
            }
        }

        private void ExecuteXoa(object obj)
        {
            if (SelectedDanhMuc == null)
            {
                DialogService.ShowError("Vui lòng chọn danh mục cần xóa");
                return;
            }

            if (DialogService.ShowConfirm($"Bạn có chắc muốn xóa '{SelectedDanhMuc.TenLoai}'?"))
            {
                if (_service.Delete(SelectedDanhMuc.ID))
                {
                    DialogService.ShowMessage("Xóa thành công");
                    LoadDanhMuc();
                }
                else
                {
                    DialogService.ShowError("Xóa thất bại");
                }
            }
        }
    }
}

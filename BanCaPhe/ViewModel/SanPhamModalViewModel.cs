using BanCaPhe.Helpers;
using BanCaPhe.Models;
using BanCaPhe.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace BanCaPhe.ViewModel
{
    public class SanPhamModalViewModel : BaseViewModel
    {
        private readonly DoUongService _service = new DoUongService();
        private int _id = 0;

        private string _tenDoUong;
        public string TenDoUong { get => _tenDoUong; set { _tenDoUong = value; OnPropertyChanged(); } }

        private string _gia;
        public string Gia { get => _gia; set { _gia = value; OnPropertyChanged(); } }

        private string _mota;
        public string Mota { get => _mota; set { _mota = value; OnPropertyChanged(); } }

        private bool _conBan;
        public bool ConBan { get => _conBan; set { _conBan = value; OnPropertyChanged(); } }

        private string _hinhAnh;
        public string HinhAnh { get => _hinhAnh; set { _hinhAnh = value; OnPropertyChanged(); UpdateImagePreview(); } }

        private BitmapImage _imagePreview;
        public BitmapImage ImagePreview { get => _imagePreview; set { _imagePreview = value; OnPropertyChanged(); } }

        private ObservableCollection<DanhMucDouong> _danhMucList;
        public ObservableCollection<DanhMucDouong> DanhMucList { get => _danhMucList; set { _danhMucList = value; OnPropertyChanged(); } }

        private DanhMucDouong _selectedDanhMuc;
        public DanhMucDouong SelectedDanhMuc { get => _selectedDanhMuc; set { _selectedDanhMuc = value; OnPropertyChanged(); } }

        public ICommand ChonHinhCommand { get; }
        public ICommand LuuCommand { get; }
        public ICommand HuyCommand { get; }

        public SanPhamModalViewModel(DoUong sp = null)
        {
            ChonHinhCommand = new RelayCommand(ExecuteChonHinh);
            LuuCommand = new RelayCommand(ExecuteLuu);
            HuyCommand = new RelayCommand(ExecuteHuy);

            LoadLoai();

            if (sp != null)
            {
                _id = sp.ID;
                TenDoUong = sp.TenDoUong;
                Gia = sp.Gia.ToString();
                Mota = sp.Mota;
                ConBan = sp.ConBan;
                HinhAnh = sp.HinhAnh;
                SelectedDanhMuc = DanhMucList.FirstOrDefault(x => x.ID == sp.LoaiID);
            }
            else
            {
                ConBan = true;
            }
        }

        private void LoadLoai()
        {
            var list = _service.GetLoai();
            DanhMucList = new ObservableCollection<DanhMucDouong>(list);
        }

        private void UpdateImagePreview()
        {
            if (string.IsNullOrEmpty(HinhAnh))
            {
                ImagePreview = null;
                return;
            }

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HinhAnh", HinhAnh);
            if (File.Exists(path))
            {
                try
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(path);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    ImagePreview = bitmap;
                }
                catch { ImagePreview = null; }
            }
            else
            {
                ImagePreview = null;
            }
        }

        private void ExecuteChonHinh(object obj)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image (*.png;*.jpg)|*.png;*.jpg";

            if (dlg.ShowDialog() == true)
            {
                string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HinhAnh");
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                string fileName = Path.GetFileName(dlg.FileName);
                string destPath = Path.Combine(folder, fileName);

                int count = 1;
                while (File.Exists(destPath))
                {
                    string name = Path.GetFileNameWithoutExtension(fileName);
                    string ext = Path.GetExtension(fileName);
                    destPath = Path.Combine(folder, $"{name}_{count}{ext}");
                    count++;
                }

                File.Copy(dlg.FileName, destPath);
                HinhAnh = Path.GetFileName(destPath);
            }
        }

        private void ExecuteLuu(object obj)
        {
            var sp = new DoUong
            {
                ID = _id,
                TenDoUong = TenDoUong?.Trim(),
                Gia = decimal.TryParse(Gia, out decimal g) ? g : -1,
                LoaiID = SelectedDanhMuc?.ID,
                Mota = Mota,
                ConBan = ConBan,
                HinhAnh = HinhAnh
            };

            if (!ValidationHelper.ValidateWithReport(sp)) return;

            try
            {
                bool success;
                if (_id == 0)
                    success = _service.Insert(sp);
                else
                    success = _service.Update(sp);

                if (success)
                {
                    DialogService.ShowMessage("Lưu sản phẩm thành công");
                    var window = obj as Window;
                    if (window != null)
                    {
                        window.DialogResult = true;
                        window.Close();
                    }
                }
                else
                {
                    DialogService.ShowError("Lưu thất bại");
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

        private void ExecuteHuy(object obj)
        {
            var window = obj as Window;
            window?.Close();
        }
    }
}

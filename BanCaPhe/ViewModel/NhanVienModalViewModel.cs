using BanCaPhe.Helpers;
using BanCaPhe.Models;
using BanCaPhe.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace BanCaPhe.ViewModel
{
    public class NhanVienModalViewModel : BaseViewModel
    {
        private readonly NhanVienService _service = new NhanVienService();
        private int? _id = null;

        private string _hoTen;
        public string HoTen { get => _hoTen; set { _hoTen = value; OnPropertyChanged(); } }

        private string _email;
        public string Email { get => _email; set { _email = value; OnPropertyChanged(); } }

        private string _soDienThoai;
        public string SoDienThoai { get => _soDienThoai; set { _soDienThoai = value; OnPropertyChanged(); } }

        private string _vaiTro;
        public string VaiTro { get => _vaiTro; set { _vaiTro = value; OnPropertyChanged(); } }

        private bool _trangThai;
        public bool TrangThai { get => _trangThai; set { _trangThai = value; OnPropertyChanged(); } }

        private string _hinhAnh;
        public string HinhAnh { get => _hinhAnh; set { _hinhAnh = value; OnPropertyChanged(); UpdateImagePreview(); } }

        private BitmapImage _imagePreview;
        public BitmapImage ImagePreview { get => _imagePreview; set { _imagePreview = value; OnPropertyChanged(); } }

        private Visibility _matKhauVisibility = Visibility.Visible;
        public Visibility MatKhauVisibility { get => _matKhauVisibility; set { _matKhauVisibility = value; OnPropertyChanged(); } }

        public ICommand ChonAnhCommand { get; }
        public ICommand LuuCommand { get; }
        public ICommand HuyCommand { get; }

        public NhanVienModalViewModel(int? id = null)
        {
            _id = id;
            ChonAnhCommand = new RelayCommand(ExecuteChonAnh);
            LuuCommand = new RelayCommand(ExecuteLuu);
            HuyCommand = new RelayCommand(ExecuteHuy);

            if (_id.HasValue)
            {
                LoadNhanVien();
                MatKhauVisibility = Visibility.Collapsed;
            }
            else
            {
                MatKhauVisibility = Visibility.Visible;
                TrangThai = true;
                VaiTro = "Employee";
            }
        }

        private void LoadNhanVien()
        {
            var nv = _service.GetById(_id.Value);
            if (nv == null) return;

            HoTen = nv.HoTen;
            Email = nv.Email;
            SoDienThoai = nv.SoDienThoai;
            VaiTro = nv.VaiTro;
            TrangThai = nv.TrangThai;
            HinhAnh = nv.HinhAnh;
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
            else { ImagePreview = null; }
        }

        private void ExecuteChonAnh(object obj)
        {
            OpenFileDialog dlg = new OpenFileDialog { Filter = "Image (*.jpg;*.png)|*.jpg;*.png" };
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
            var passwordBox = obj as System.Windows.Controls.PasswordBox;
            
            var nv = new NhanVien
            {
                ID = _id ?? 0,
                HoTen = HoTen?.Trim(),
                Email = Email?.Trim(),
                SoDienThoai = SoDienThoai?.Trim(),
                VaiTro = VaiTro,
                TrangThai = TrangThai,
                HinhAnh = HinhAnh
            };

            if (_id == null)
            {
                string rawPassword = passwordBox?.Password;
                if (string.IsNullOrWhiteSpace(rawPassword))
                {
                    DialogService.ShowError("Mật khẩu không được để trống");
                    return;
                }
                if (rawPassword.Length < 6)
                {
                    DialogService.ShowError("Mật khẩu phải từ 6 ký tự");
                    return;
                }
                nv.MatKhau = PasswordHelper.HashPassword(rawPassword);
            }
            else
            {
                var nvCu = _service.GetById(_id.Value);
                if (nvCu == null)
                {
                    DialogService.ShowError("Không tìm thấy thông tin nhân viên!");
                    return;
                }
                nv.MatKhau = nvCu.MatKhau;
            }

            if (!ValidationHelper.ValidateWithReport(nv)) return;

            bool result = false;
            try
            {
                result = _id == null ? _service.Insert(nv) : _service.Update(nv);
            }
            catch (Exception ex)
            {
                DialogService.ShowError(ex.Message);
                return;
            }

            if (result)
            {
                DialogService.ShowMessage("Lưu nhân viên thành công");
                // Find and close window
                var window = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is Views.W_NhanVienModal);
                if (window != null)
                {
                    window.DialogResult = true;
                    window.Close();
                }
            }
            else
            {
                DialogService.ShowError("Lưu thất bại. Vui lòng kiểm tra lại thông tin.");
            }
        }

        private void ExecuteHuy(object obj)
        {
            var window = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is Views.W_NhanVienModal);
            window?.Close();
        }
    }
}

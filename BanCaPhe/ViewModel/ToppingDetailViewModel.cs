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
    public class ToppingDetailViewModel : BaseViewModel
    {
        private readonly ToppingService _service = new ToppingService();
        public Topping Topping { get; }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        private BitmapImage _imagePreview;
        public BitmapImage ImagePreview
        {
            get => _imagePreview;
            set { _imagePreview = value; OnPropertyChanged(); }
        }

        public ICommand LuuCommand { get; }
        public ICommand HuyCommand { get; }
        public ICommand ChonAnhCommand { get; }

        public ToppingDetailViewModel(Topping topping)
        {
            Topping = topping ?? new Topping { ConBan = true };

            LuuCommand = new RelayCommand(ExecuteLuu);
            HuyCommand = new RelayCommand(ExecuteHuy);
            ChonAnhCommand = new RelayCommand(ExecuteChonAnh);

            UpdateImagePreview();
        }

        private void UpdateImagePreview()
        {
            if (string.IsNullOrEmpty(Topping.HinhAnh)) return;

            try
            {
                if (File.Exists(Topping.HinhAnh))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(Topping.HinhAnh);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    ImagePreview = bitmap;
                }
            }
            catch { }
        }

        private void ExecuteChonAnh(object obj)
        {
            OpenFileDialog dlg = new OpenFileDialog { Filter = "Image (*.jpg;*.png)|*.jpg;*.png" };
            if (dlg.ShowDialog() == true)
            {
                Topping.HinhAnh = dlg.FileName;
                UpdateImagePreview();
            }
        }

        private void ExecuteLuu(object obj)
        {
            ErrorMessage = string.Empty;

            // Validate bằng Attribute
            if (!ValidationHelper.IsValid(Topping, out List<string> errors))
            {
                ErrorMessage = string.Join("\n", errors);
                return;
            }

            try
            {
                bool result = false;
                if (Topping.ID == 0)
                {
                    result = _service.Insert(Topping);
                }
                else
                {
                    result = _service.Update(Topping);
                }

                if (result)
                {
                    DialogService.ShowMessage("Lưu Topping thành công!");
                    CloseWindow(true);
                }
                else
                {
                    ErrorMessage = "Lưu Topping thất bại!";
                }
            }
            catch (Exception ex)
            {
                // Hiển thị lỗi từ SQL ngay lên giao diện thông qua thuộc tính ErrorMessage
                ErrorMessage = ex.Message;
            }
        }

        private void ExecuteHuy(object obj)
        {
            CloseWindow(false);
        }

        private void CloseWindow(bool result)
        {
            var window = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.DataContext == this);
            if (window != null)
            {
                window.DialogResult = result;
                window.Close();
            }
        }
    }
}

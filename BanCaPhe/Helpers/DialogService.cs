using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BanCaPhe.Helpers
{
    public interface IDialogService
    {
        void ShowMessage(string message, string title = "Thông báo");
        void ShowError(string message, string title = "Lỗi");
        bool ShowConfirm(string message, string title = "Xác nhận");
    }

    public class DialogService : IDialogService
    {
        public void ShowMessage(string message, string title = "Thông báo")
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void ShowError(string message, string title = "Lỗi")
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public bool ShowConfirm(string message, string title = "Xác nhận")
        {
            return MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }
    }
}

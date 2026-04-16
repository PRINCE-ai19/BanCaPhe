using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BanCaPhe.Services
{
    public interface IWindowService
    {
        void ShowWindow(Window window);
        void CloseWindow(Window window);
        void ShowAdminDashboard();
        void ShowMainWindow();
        void ShowLoginWindow();
        void ShowRegisterWindow();
    }

    public class WindowService : IWindowService
    {
        public void ShowWindow(Window window)
        {
            window.Show();
        }

        public void CloseWindow(Window window)
        {
            window.Close();
        }

        public void ShowAdminDashboard()
        {
            var admin = new AdminDashboard();
            admin.Show();
        }

        public void ShowMainWindow()
        {
            var main = new MainWindow();
            Application.Current.MainWindow = main;
            main.Show();
        }

        public void ShowLoginWindow()
        {
            var login = new W_DangNhap();
            login.Show();
        }

        public void ShowRegisterWindow()
        {
            var register = new DangKy();
            register.Show();
        }
    }
}

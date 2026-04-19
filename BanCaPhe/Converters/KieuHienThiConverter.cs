using BanCaPhe.ViewModel;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BanCaPhe.Converters
{

    public class KieuHienThiConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is KieuHienThi kieuHienThi && parameter is string paramStr)
            {
        
                if (paramStr == "DoUong")
                    return kieuHienThi == KieuHienThi.DoUong ? Visibility.Visible : Visibility.Collapsed;
                
            
                if (paramStr == "Topping")
                    return kieuHienThi == KieuHienThi.Topping ? Visibility.Visible : Visibility.Collapsed;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

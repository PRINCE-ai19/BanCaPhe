using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;  
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace BanCaPhe.Converters
{
    public class ImagePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            string fileName = value.ToString();
            if (string.IsNullOrEmpty(fileName))
                return null;

            string fullPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "HinhAnh",
                fileName
            );

            if (!File.Exists(fullPath))
                return null;

            return new BitmapImage(new Uri(fullPath));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }




    }
}

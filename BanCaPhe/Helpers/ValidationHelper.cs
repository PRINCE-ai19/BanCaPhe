using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanCaPhe.Helpers
{
    public static class ValidationHelper
    {
    
        public static bool IsValid<T>(T obj, out List<string> errorMessages)
        {
            errorMessages = new List<string>();
            var results = new List<ValidationResult>();
            var context = new ValidationContext(obj);

            bool isValid = Validator.TryValidateObject(obj, context, results, true);

            if (!isValid)
            {
                errorMessages = results.Select(r => r.ErrorMessage).ToList();
            }

            return isValid;
        }

   
        public static bool ValidateWithReport<T>(T obj)
        {
            if (!IsValid(obj, out List<string> errors))
            {
                System.Windows.MessageBox.Show(
                    string.Join("\n", errors), 
                    "Lỗi dữ liệu", 
                    System.Windows.MessageBoxButton.OK, 
                    System.Windows.MessageBoxImage.Warning
                );
                return false;
            }
            return true;
        }
    }
}

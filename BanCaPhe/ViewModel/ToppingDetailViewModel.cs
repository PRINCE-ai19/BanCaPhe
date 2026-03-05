using BanCaPhe.Models;
using BanCaPhe.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace BanCaPhe.ViewModel
{

    public class ToppingDetailViewModel : BaseViewModel
    {
        public Topping Topping { get; }

        public ToppingDetailViewModel(Topping topping)
        {
            if (topping == null)
                throw new ArgumentNullException(nameof(topping), "Topping không được null");

            Topping = topping;
        }
    }
}

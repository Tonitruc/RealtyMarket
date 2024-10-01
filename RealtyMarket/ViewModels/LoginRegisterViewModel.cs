using System.Windows.Input;
using RealtyMarket.Models;
using MvvmHelpers;
using MvvmHelpers.Commands;

namespace RealtyMarket.ViewModels
{
    public class LoginRegisterViewModel : ObservableObject
    {
        public UserLoginInfo User { get; set; }

        public ICommand EnterAsGuestCommand { get; }

        public ICommand LoginCommand { get; }

        public ICommand RegisterCommand { get; }

        public LoginRegisterViewModel()
        {
            User = new UserLoginInfo();
            EnterAsGuestCommand = new AsyncCommand(EnterAsGuest);
        }

        public static async Task EnterAsGuest()
        {
            await SecureStorage.SetAsync("UserStatus", "Guest");
            await Shell.Current.GoToAsync("//CatalogPage");
        }
    }
}

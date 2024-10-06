using MvvmHelpers;
using MvvmHelpers.Commands;
using RealtyMarket.Service;
using System.Windows.Input;

namespace RealtyMarket.ViewModels
{
    public class ProfileViewModel : ObservableObject
    {
        private readonly FirebaseAuthenticationService _authService;

        public ICommand SignOutCommand { get; set; }


        public ProfileViewModel(FirebaseAuthenticationService authService)
        {
            _authService = authService;
            SignOutCommand = new MvvmHelpers.Commands.Command(SignOut);
        }

        public async void SignOut()
        {
            _authService.SignOutUser();
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}

using Firebase.Auth;
using MvvmHelpers;
using MvvmHelpers.Commands;
using RealtyMarket.Models;
using RealtyMarket.Service;
using System.Windows.Input;

namespace RealtyMarket.ViewModels
{
    public class ProfileViewModel : ObservableObject
    {
        private readonly FirebaseAuthenticationService _authService;

        private readonly SecureStorageUserRepository _userRepository;

        private bool _isRegisteredUser;

        public bool IsRegisteredUser
        {
            get => _isRegisteredUser;
            set => SetProperty(ref _isRegisteredUser, value);
        }

        public ICommand SignOutCommand { get; set; }

        public string _email;
        
        public string Email
        {
            get => _email;
            set => SetProperty<string>(ref _email, value);
        }

        public ProfileViewModel(FirebaseAuthenticationService authService,
            SecureStorageUserRepository userRepository)
        {
            _authService = authService;
            SignOutCommand = new AsyncCommand(SignOut);
            _userRepository = userRepository;
        }

        public async Task InitializeUserStateAsync()
        {
            string userState = await _userRepository.GetUserState();
            if (userState == "Register")
            {
                IsRegisteredUser = true;
                var user = await _userRepository.ReadUser();
                Email = user.userInfo.Email;
            }
            else
            {
                IsRegisteredUser = false;
                Email = "Unknown";
            }
        }

        public async Task SignOut()
        {
            await _authService.SignOutUser();
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}

using Firebase.Auth;
using MvvmHelpers;
using MvvmHelpers.Commands;
using RealtyMarket.Models;
using RealtyMarket.Repository;
using RealtyMarket.Service;
using System.Windows.Input;

namespace RealtyMarket.ViewModels
{
    public class ProfileViewModel : ObservableObject
    {
        private readonly FirebaseAuthenticationService _authService;

        private readonly SecureStorageUserRepository _userRepository;

        private readonly RegisteredUserRepository _registeredUserRepository;

        private readonly ImageBBRepository _imageBBRepository;

        private bool _isRegisteredUser;

        public bool IsRegisteredUser
        {
            get => _isRegisteredUser;
            set => SetProperty(ref _isRegisteredUser, value);
        }

        public ICommand SignOutCommand { get; }
        public ICommand GetMyAdsCommand { get; }
        public ICommand GetMyFavoritesCommand { get; }
        public ICommand ChangeUserSettingsCommand { get; }

        public string _email;
        
        public string Email
        {
            get => _email;
            set => SetProperty<string>(ref _email, value);
        }

        private ImageSource _avatarPhoto;

        public ImageSource AvatarPhoto
        {
            get => _avatarPhoto;
            set => SetProperty(ref _avatarPhoto, value);
        }

        private bool _isLoading = false;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }


        public ProfileViewModel(FirebaseAuthenticationService authService,
            SecureStorageUserRepository userRepository, RegisteredUserRepository registeredUserRepository,
            ImageBBRepository imageBBRepository)
        {
            SignOutCommand = new AsyncCommand(SignOut);
            GetMyAdsCommand = new AsyncCommand(GetMyAds);
            GetMyFavoritesCommand = new AsyncCommand(GetMyFavorites);
            ChangeUserSettingsCommand = new AsyncCommand(ChangeUserSettings);

            _authService = authService;
            _userRepository = userRepository;
            _registeredUserRepository = registeredUserRepository;
            _imageBBRepository = imageBBRepository;
        }

        public async Task InitializeUserStateAsync()
        {
            string userState = await _userRepository.GetUserState();
            if (userState == "Register")
            {
                IsRegisteredUser = true;
                var user = await _userRepository.ReadUser();
                Email = user.userInfo.Email;
                var regUser = await _registeredUserRepository.GetByEmail(Email);
                if(!string.IsNullOrEmpty(regUser.UserImageUrl))
                {
                    AvatarPhoto = _imageBBRepository.Get(regUser.UserImageUrl);
                }
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

        public async Task GetMyAds()
        {
            await Shell.Current.GoToAsync("//MyAdPage");
        }

        public async Task GetMyFavorites()
        {
            await Shell.Current.GoToAsync("//FavoritesPage");
        }

        public async Task ChangeUserSettings()
        {
            await Shell.Current.GoToAsync("//UserSettingsPage");
        }
    }
}

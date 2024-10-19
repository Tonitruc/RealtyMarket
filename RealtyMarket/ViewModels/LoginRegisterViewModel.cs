using System.Windows.Input;
using RealtyMarket.Models;
using MvvmHelpers;
using MvvmHelpers.Commands;
using RealtyMarket.Service;
using RealtyMarket.Repository;

namespace RealtyMarket.ViewModels
{
    public class LoginRegisterViewModel : ObservableObject
    {
        public UserLoginInfo User { get; set; }

        public UserRegisterInfo NewUser { get; set; }

        public ICommand EnterAsGuestCommand { get; }

        public ICommand LoginCommand { get; }

        public ICommand RegisterCommand { get; }

        private string _loginErrorMessage = string.Empty;

        public string LoginErrorMessage
        {
            get => _loginErrorMessage;
            set => SetProperty(ref _loginErrorMessage, value);
        }

        private bool _isLoginError = false;

        public bool IsLoginError
        {
            get => _isLoginError;
            set => SetProperty(ref _isLoginError, value);
        }

        private string _registerErrorMessage = string.Empty;

        public string RegisterErrorMessage
        {
            get => _registerErrorMessage;
            set => SetProperty(ref _registerErrorMessage, value);
        }

        private bool _isRegisterError = false;

        public bool IsRegisterError
        {
            get => _isRegisterError;
            set => SetProperty(ref _isRegisterError, value);
        }

        private bool _isBusy = false;

        public bool IsBusy
        {
            get => !_isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        private readonly FirebaseAuthenticationService _authService;

        private readonly RegisteredUserRepository _registeredUserRepository;

        private readonly SecureStorageUserRepository _secureStorageUserRepository;

        public LoginRegisterViewModel(FirebaseAuthenticationService authService,
            RegisteredUserRepository registeredUserRepository, SecureStorageUserRepository secureStorageUserRepository)
        {
            User = new UserLoginInfo();
            NewUser = new UserRegisterInfo();
            EnterAsGuestCommand = new AsyncCommand(EnterAsGuest);
            RegisterCommand = new AsyncCommand(Register);
            LoginCommand = new AsyncCommand(Login);

            _authService = authService;
            _registeredUserRepository = registeredUserRepository;
            _secureStorageUserRepository = secureStorageUserRepository;
        }

        public async Task EnterAsGuest()
        {
            IsBusy = true;

            SignInResultEnum result = await _authService.SignInAnonymousUserAsync();
            if (result == SignInResultEnum.Ok)
            {
                IsLoginError = false;
                await Shell.Current.GoToAsync("//CatalogPage");
            }
            else if (result == SignInResultEnum.InValidError)
            {
                LoginErrorMessage = "Непредвиденная ошибка.";
                IsLoginError = true;
            }
            IsBusy = false;
        }

        public async Task Register()
        {
            IsBusy = true;
            RegisteredUser existUser = await _registeredUserRepository.GetByEmail(NewUser.Email);

            if (existUser == null)
            {
                SignInResultEnum result = await _authService.RegisterUserAsync(NewUser.Email, NewUser.Password);
                if (result == SignInResultEnum.Ok)
                {
                    IsRegisterError = false;
                    await Shell.Current.GoToAsync("//MainTabs");
                    await _registeredUserRepository.Add(new() { Email = NewUser.Email, Password = NewUser.Password });
                }
                else if (result == SignInResultEnum.EmailIsBusy)
                {
                    RegisterErrorMessage = "Email уже занят.";
                    IsRegisterError = true;
                }
                else if (result == SignInResultEnum.InValidError)
                {
                    RegisterErrorMessage = "Непредвиденная ошибка.";
                    IsRegisterError = true;
                }
            }
            else
            {
                RegisterErrorMessage = "Email уже занят.";
                IsRegisterError = true;
            }
            IsBusy = false;
        }

        public async Task Login()
        {
            IsBusy = true;

            RegisteredUser existUser =  await _registeredUserRepository.GetByEmail(User.Email);

            if (existUser != null)
            {
                SignInResultEnum result = await _authService.SignInUserAsync(User.Email, User.Password);
                if (result == SignInResultEnum.Ok)
                {
                    IsLoginError = false;
                    await Shell.Current.GoToAsync("//MainTabs");
                }
                else if (result == SignInResultEnum.EmailNotExist)
                {
                    LoginErrorMessage = "Неверный пароль.";
                    IsLoginError = true;
                }
                else if (result == SignInResultEnum.InValidError)
                {
                    LoginErrorMessage = "Непредвиденная ошибка.";
                    IsLoginError = true;
                }
            }
            else
            {
                LoginErrorMessage = "Пользователь не найден, пройдите регистрацию.";
                IsLoginError = true;
            }
            IsBusy = false;
        }
    }
}

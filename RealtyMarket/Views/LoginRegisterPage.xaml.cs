using RealtyMarket.Controls;
using System.Collections.ObjectModel;
using RealtyMarket.Service;
using RealtyMarket.ViewModels;

namespace RealtyMarket.Views
{
    public partial class LoginRegisterPage : ContentPage
    {

        private readonly LoginRegisterViewModel _loginRegisterViewModel;

        private readonly ConnectivityService _connectivityService;

        public LoginRegisterPage(LoginRegisterViewModel loginRegisterViewModel, 
            ConnectivityService connectivityService)
        {
            InitializeComponent();

            _connectivityService = connectivityService;
            _loginRegisterViewModel = loginRegisterViewModel;
            BindingContext = _loginRegisterViewModel;
        }

        private void EmailTextChanged(object sender, EventArgs e)
        {
            if(sender is CustomEntry entry)
            {
                string email = entry.Text;

                if (string.IsNullOrEmpty(entry.Text))
                {
                    entry.IsError = true;
                    entry.ErrorText = "Поле не может быть пустым";
                }
                else if (!Validators.IsValidEmail(email))
                {
                    entry.IsError = true;
                    entry.ErrorText = "Неверный формат e-mail";
                }
                else
                {
                    entry.IsError = false;
                }
            }
        }

        private void LoginPasswordTextChanged(object sender, EventArgs e)
        {
            string password = LoginPasswordEntry.Text;

            if (string.IsNullOrEmpty(password))
            {
                LoginPasswordEntry.IsError = true;
                LoginPasswordEntry.ErrorText = "Поле не может быть пустым";
            }
            else if(password.Length < 8)
            {
                LoginPasswordEntry.IsError = true;
                LoginPasswordEntry.ErrorText = "Минимум 8 символов.";
            }
            else
            {
                LoginPasswordEntry.IsError = false;
            }
        }

        private void RegisterPasswordTextChanged(object sender, EventArgs e)
        {
            string password = RegisterPasswordEntry.Text;

            if (!string.IsNullOrEmpty(password))
            {
                bool isMistake = true;
                RegistrationDigits.TextColor = (isMistake &= password.Any(Char.IsDigit)) ? Colors.Green : Colors.Red;
                RegistrationLetters.TextColor = (isMistake &= password.Any(Char.IsLetter)) ? Colors.Green : Colors.Red;
                RegistrationLenght.TextColor = (isMistake &= password.Length >= 8) ? Colors.Green : Colors.Red;
                RegisterPasswordEntry.IsError = !isMistake;
                RegisterPasswordEntry.ErrorText = "Тебования не соблюдены";
            }
            else
            {
                RegistrationDigits.TextColor = Colors.Red;
                RegistrationLetters.TextColor = Colors.Red;
                RegistrationLenght.TextColor = Colors.Red;
                RegisterPasswordEntry.IsError = true;
                RegisterPasswordEntry.ErrorText = "Пароль не должен быть пустым";
            }

            AllowPasswordChanged(RegisterAllowPasswordEntry, e);
        }

        private void AllowPasswordChanged(object sender, EventArgs e)
        {
            string allowPassword = RegisterAllowPasswordEntry.Text;
            string password = RegisterPasswordEntry.Text;

            if (!allowPassword.Equals(password))
            {
                RegisterAllowPasswordEntry.ErrorText = "Пароли должны совпадать";
                RegisterAllowPasswordEntry.IsError = true;
            }
            else
            {
                RegisterAllowPasswordEntry.IsError = false;
            }
        }

        public void RegistrationButtonClicked(object sender, EventArgs e)
        {
            RegisterPasswordTextChanged(RegisterPasswordEntry, e);
            EmailTextChanged(RegisterEmailEntry, e);
            AllowPasswordChanged(RegisterAllowPasswordEntry, e);

            if (!(RegisterPasswordEntry.IsError
                && RegisterEmailEntry.IsError
                && RegisterAllowPasswordEntry.IsError))
            {
                _loginRegisterViewModel.RegisterCommand.Execute(this);
            }
        }

        public void LoginButtonClicked(object sender, EventArgs e)
        {
            LoginPasswordTextChanged(LoginPasswordEntry, e);
            EmailTextChanged(LoginEmailEntry, e);

            if (!(LoginPasswordEntry.IsError && LoginEmailEntry.IsError))
            {
                _loginRegisterViewModel.LoginCommand.Execute(this);
            }
        }

        public void EnterAsGuest(object sender, EventArgs e)
        {
            _loginRegisterViewModel.EnterAsGuestCommand.Execute(this);
        }

        private void OnSizeChanged(object sender, EventArgs e)
        {
            bool isLandscape = (Width > Height);

            int fontSize = isLandscape ? 20 : 13;

            ObservableCollection<RegistrationAdvantage> items = [
                    new() { Text = "Возможность публикации объявлений.", FontSize = fontSize},
                new() { Text = "Личный кабинет.", FontSize = fontSize},
                new() { Text = "Избранное и история.", FontSize = fontSize}
                ];

            LoginAdvanatages.ItemsSource = items;
            RegisterAdvantages.ItemsSource = items;
        }
    }
}
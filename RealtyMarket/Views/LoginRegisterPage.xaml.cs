using Microsoft.Maui.Controls;
using RealtyMarket.Controls;
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

        private async void RegisterPageButtonClicked(object sender, EventArgs e)
        {
            App.Current.Resources.TryGetValue("Primary", out var colorvalue);

            LoginPageButton.TextColor = Colors.Black;
            RegisterPageButton.TextColor = (Color)colorvalue;

            RegisterPage.TranslationX = Width;
            RegisterPage.IsVisible = true;

            var translateBoxViewTask = SelectBoxView.TranslateTo(SelectBoxView.Width, 0, 200, Easing.Linear);
            var translateLoginPageTask = LoginPage.TranslateTo(-Width, 0, 200, Easing.Linear);
            var translateRegisterPageTask = RegisterPage.TranslateTo(0, 0, 200, Easing.Linear);

            await Task.WhenAll(translateBoxViewTask, translateLoginPageTask, translateRegisterPageTask);

            LoginPage.IsVisible = false;
        }

        private async void LoginPageButtonClicked(object sender, EventArgs e)
        {
            App.Current.Resources.TryGetValue("Primary", out var colorvalue);

            RegisterPageButton.TextColor = Colors.Black;
            LoginPageButton.TextColor = (Color)colorvalue;

            LoginPage.TranslationX = -Width;
            LoginPage.IsVisible = true;

            var translateBoxViewTask = SelectBoxView.TranslateTo(0, 0, 200, Easing.Linear);
            var translateLoginPageTask = LoginPage.TranslateTo(0, 0, 200, Easing.Linear);
            var translateRegisterPageTask = RegisterPage.TranslateTo(Width, 0, 200, Easing.Linear);

            await Task.WhenAll(translateBoxViewTask, translateLoginPageTask, translateRegisterPageTask);

            RegisterPage.IsVisible = false;
        }

        private void EmailTextChanged(object sender, EventArgs e)
        {
            if (sender is GrEntry entry)
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
            else if (password.Length < 8)
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
                RegisterPasswordEntry.ErrorText = "Требования не соблюдены";
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

            if (!allowPassword.Equals(password) || string.IsNullOrEmpty(password))
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
                || RegisterEmailEntry.IsError
                || RegisterAllowPasswordEntry.IsError))
            {
                _loginRegisterViewModel.RegisterCommand.Execute(this);
            }
        }

        public void LoginButtonClicked(object sender, EventArgs e)
        {
            LoginPasswordTextChanged(LoginPasswordEntry, e);
            EmailTextChanged(LoginEmailEntry, e);

            if (!(LoginPasswordEntry.IsError || LoginEmailEntry.IsError))
            {
                _loginRegisterViewModel.LoginCommand.Execute(this);
            }
        }

        public void EnterAsGuest(object sender, EventArgs e)
        {
            _loginRegisterViewModel.EnterAsGuestCommand.Execute(this);
        }
    }
}
using RealtyMarket.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace RealtyMarket;

public partial class LoginPage : ContentPage
{
    private bool _isPasswordVisible;

    public LoginPage()
    {
        InitializeComponent();
    }

    private void EmailTextChanged(object sender, TextChangedEventArgs e)
    {
        string email = EmailEntry.Text;

        if (string.IsNullOrEmpty(EmailEntry.Text))
        {
            EmailEntry.IsError = true;
            EmailEntry.ErrorText = "���� �� ����� ���� ������";
        }
        else if(!Validators.IsValidEmail(email))
        {
            EmailEntry.IsError = true;
            EmailEntry.ErrorText = "�������� ������ e-mail";
        }
        else
        {
            EmailEntry.IsError = false;
        }
    }

    private void PasswordTextChanged(object sender, TextChangedEventArgs e)
    {
        string password = PasswordEntry.Text;

        if (!string.IsNullOrEmpty(password))
        {
            bool isMistake = true;
            RegistrationDigits.TextColor = (isMistake &= password.Any(Char.IsDigit)) ? Colors.Green : Colors.Red;
            RegistrationLetters.TextColor = (isMistake &= password.Any(Char.IsLetter)) ? Colors.Green : Colors.Red;
            RegistrationLenght.TextColor = (isMistake &= password.Length >= 8) ? Colors.Green : Colors.Red;
            PasswordEntry.IsError = !isMistake;
            PasswordEntry.ErrorText = "��������� �� ���������";
        }
        else
        {
            RegistrationDigits.TextColor = Colors.Red;
            RegistrationLetters.TextColor = Colors.Red;
            RegistrationLenght.TextColor = Colors.Red;
            PasswordEntry.IsError = true;
            PasswordEntry.ErrorText = "������ �� ������ ���� ������";
        }
    }

    private void AllowPasswordChanged(object sender, TextChangedEventArgs e)
    {
        string allowPassword = AllowPasswordEntry.Text;
        string password = PasswordEntry.Text;

        if(!allowPassword.Equals(password))
        {
            AllowPasswordEntry.ErrorText = "������ ������ ���������";
            AllowPasswordEntry.IsError = true;
        }
        else
        {
            AllowPasswordEntry.IsError = false;
        }
    }

    private void OnSizeChanged(object sender, EventArgs e)
    {
        bool isLandscape = (Width > Height);

        int fontSize = isLandscape ? 20 : 13;

        ObservableCollection<RegistrationAdvantage> items = [
                new() { Text = "����������� ���������� ����������.", FontSize = fontSize},
                new() { Text = "������ �������.", FontSize = fontSize},
                new() { Text = "��������� � �������.", FontSize = fontSize}
            ];

        LoginAdvanatages.ItemsSource = items;
        RegisterAdvantages.ItemsSource = items;
    }
}
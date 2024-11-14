using RealtyMarket.Controls;
using RealtyMarket.Service;
using RealtyMarket.ViewModels;
using System.Text.RegularExpressions;

namespace RealtyMarket.Views;

public partial class UserSettingsPage : ContentPage
{
    private readonly UserSettingsViewModel _viewModel;

	public UserSettingsPage(UserSettingsViewModel viewModel)
	{
		InitializeComponent();

        BindingContext = _viewModel = viewModel;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        _numberCnahged = false;

        _viewModel.IsLoading = true;

        await _viewModel.GetUserInfo();

        if(!string.IsNullOrEmpty(_viewModel.User.Number))
        {
            NumberEntry.Text = _viewModel.User.Number;
        }

        _viewModel.IsLoading = false;
    }

    private void PhoneNumberChanged(object sender, TextChangedEventArgs e)
    {
        GrEntry entry = NumberEntry;

        if (e.NewTextValue.Length >= 6 && e.NewTextValue.Length < e.OldTextValue.Length)
        {
            return;
        }

        if (e.NewTextValue.Length < 6
            || e.NewTextValue.Length >= 20)
        {
            entry.Text = e.OldTextValue;
            return;
        }

        var numericText = Regex.Replace(e.NewTextValue, "[^0-9]", "");

        string formatedText = "+375 (";

        for(int i = 0; i < numericText.Length; i++)
        {
            if(i == 3 || i == 5 || i == 6 || i == 8 || i == 10 || i == 11)
            {
                formatedText += numericText[i];
            }
            if(i == 4)
            {
                formatedText += numericText[i] + ") ";
            }
            if (i == 7)
            {
                formatedText += numericText[i] + "-";
            }
            if (i == 9)
            {
                formatedText += numericText[i] + "-";
            }
        }

        if(entry.Text != formatedText)
        {
            entry.Text = formatedText;
            entry.CursorPosition = formatedText.Length;
            _numberCnahged = true;
        }
    }

    private bool _numberCnahged = false;

    private async void SaveChanges(object sender, EventArgs e)
    {
        if(!Validators.IsValidPhoneNumber(NumberEntry.Text) && _numberCnahged)
        {
            await DisplayAlert("Ошибка", "Неверный формат номера", "Ок");
            return;
        }

        _viewModel.User.Number = NumberEntry.Text;

        await _viewModel.SaveChanges();
    }
}
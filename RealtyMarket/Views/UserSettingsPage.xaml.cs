using RealtyMarket.Service;
using RealtyMarket.ViewModels;

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

        _viewModel.IsLoading = true;

        await _viewModel.GetUserInfo();

        _viewModel.IsLoading = false;
    }
}
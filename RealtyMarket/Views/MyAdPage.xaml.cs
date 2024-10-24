using RealtyMarket.Controls;
using RealtyMarket.Models;
using RealtyMarket.ViewModels;
using Syncfusion.Maui.Popup;
using System.Windows.Input;

namespace RealtyMarket.Views;

public partial class MyAdPage : ContentPage
{
	private readonly MyAdPageViewModel _viewModel;

    public MyAdPage(MyAdPageViewModel viewModel)
	{
		InitializeComponent();

        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        _viewModel.IsLoading = true;

        await _viewModel.GetUserEmail();
        await _viewModel.GetMyAds();

        _viewModel.IsLoading = false;
    }

    private async void LeftSwipe(object sender, SwipedEventArgs e)
    {
        await CloseButtonAnimation();
    }

    private async void ClosedPageButtonClicked(object sender, TappedEventArgs e)
    {
        await CloseButtonAnimation();
    }

    private async Task CloseButtonAnimation()
    {
        if (ClosedAdPage.IsVisible)
        {
            return;
        }

        App.Current.Resources.TryGetValue("DarkViolet", out var colorvalue);

        ClosedButtonLabel.TextColor = (Color)colorvalue;
        ActiveButtonLabel.TextColor = Colors.Black;

        ClosedAdPage.TranslationX = Width;
        ClosedAdPage.IsVisible = true;

        var translateBoxViewTask = SelectBoxView.TranslateTo(SelectBoxView.Width, 0, 200, Easing.Linear);
        var translateLoginPageTask = ActiveAdPage.TranslateTo(-Width, 0, 200, Easing.Linear);
        var translateRegisterPageTask = ClosedAdPage.TranslateTo(0, 0, 200, Easing.Linear);

        await Task.WhenAll(translateBoxViewTask, translateLoginPageTask, translateRegisterPageTask);

        ActiveAdPage.IsVisible = false;
    }

    private async void RightSwipe(object sender, SwipedEventArgs e)
    {
        await ActiveButtonAnimation();
    }

    private async void ActivePageButtonClicked(object sender, TappedEventArgs e)
    {
        await ActiveButtonAnimation();
    }

    private async Task ActiveButtonAnimation()
    {
        if (ActiveAdPage.IsVisible)
        {
            return;
        }

        App.Current.Resources.TryGetValue("Primary", out var colorvalue);

        ClosedButtonLabel.TextColor = Colors.Black;
        ActiveButtonLabel.TextColor = (Color)colorvalue;

        ActiveAdPage.TranslationX = -Width;
        ActiveAdPage.IsVisible = true;

        var translateBoxViewTask = SelectBoxView.TranslateTo(0, 0, 200, Easing.Linear);
        var translateLoginPageTask = ActiveAdPage.TranslateTo(0, 0, 200, Easing.Linear);
        var translateRegisterPageTask = ClosedAdPage.TranslateTo(Width, 0, 200, Easing.Linear);

        await Task.WhenAll(translateBoxViewTask, translateLoginPageTask, translateRegisterPageTask);

        ClosedAdPage.IsVisible = false;
    }

    private async void ImageButtonClicked(object sender, EventArgs e)
    {
        var button = (ImageButton)sender;

        await button.RotateTo(90, 100, Easing.Linear);

        var frame = (Grid)button.Parent;
        var popup = frame.FindByName<SfPopup>("PopupMenu");  

        popup.ShowRelativeToView(button, PopupRelativePosition.AlignBottom);
    }

    private async void PopupMenuClosed(object sender, EventArgs e)
    {
        var popup = (SfPopup)sender;

        var frame = (Grid)popup.Parent;
        var button = frame.FindByName<ImageButton>("PopupImageButton");

        await button.RotateTo(0, 100, Easing.Linear);
    }

    private async void CloseAdClicked(object sender, EventArgs e)
    {
        var button = (GrButton)sender;

        var commandParameter = (Advertisement)button.CommandParameter;

        await _viewModel.CloseAdCommand(commandParameter);
    }

    private async void OpenAdClicked(object sender, EventArgs e)
    {
        var button = (GrButton)sender;

        var commandParameter = (Advertisement)button.CommandParameter;

        await _viewModel.OpenAdCommand(commandParameter);
    }

    private async void DeleteAdClicked(object sender, EventArgs e)
    {
        var button = (GrButton)sender;

        var commandParameter = (Advertisement)button.CommandParameter;

        await _viewModel.DeleteAdCommand(commandParameter);
    }
}
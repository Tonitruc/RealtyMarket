using RealtyMarket.ViewModels;

namespace RealtyMarket.Views;

public partial class FilterPage : ContentPage
{
	public FilterPage()
	{
		InitializeComponent();

        BindingContext = new FilterViewModel();

        this.TranslationX = Application.Current.MainPage.Width;
        CategoryComboBox.SelectedIndex = 0;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await this.TranslateTo(0, 0, 500, Easing.SinInOut);
    }

    private async void ReturnClicked(object sender, EventArgs e)
    {
        await this.TranslateTo(Application.Current.MainPage.Width, 0, 500, Easing.SinInOut);
        await Navigation.PopAsync();
    }
}
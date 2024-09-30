namespace RealtyMarket;

public partial class LoginPage : ContentPage
{
    private bool _isPasswordVisible;

    public LoginPage()
    {
        InitializeComponent();
    }

    public async void EnterAsGuest(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//CatalogPage");
    }

/*    private void OnSizeChanged(object sender, EventArgs e)
    {
        var viewModel = (LoginPageViewModel)BindingContext;
        bool isLandscape = (Width > Height);

       foreach (var item in LoginAdvanatages.ItemsSource)
       {
           item.FontSize = isLandscape ? 20 : 13;
       }
    }*/
}

public class UserLoginInfo
{
    public string Login { get; set; }

    public string Password { get; set; }
}

public class LoginPageViewModel 
{
    public LoginPageViewModel()
    {
    }
}
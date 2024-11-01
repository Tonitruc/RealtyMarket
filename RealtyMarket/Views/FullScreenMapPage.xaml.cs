namespace RealtyMarket.Views;

public partial class FullScreenMapPage : ContentPage
{
    public FullScreenMapPage(string htmlContent)
    {
        InitializeComponent();
        FullScreenWebView.Source = new HtmlWebViewSource { Html = htmlContent };
    }
}
using Microsoft.Maui.Platform;
using RealtyMarket.Models.OtherEntity;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace RealtyMarket.Views;

public partial class MapPage : ContentPage
{
    private TaskCompletionSource<RealtyLocation> _taskCompletionSource;

    private RealtyLocation _lastLocation;

    public MapPage()
    {
        InitializeComponent();

        Shell.SetTabBarIsVisible(this, false);

        MapWebView.Navigating += OnWebViewNavigating;

        LoadHtmlToWebView();
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        if (_lastLocation != null)
        {
            await MapWebView.EvaluateJavaScriptAsync($"waitForMapReadyAndSetCoordinates({_lastLocation.Latinude}, {_lastLocation.Longitude});");
        }

        _taskCompletionSource = new TaskCompletionSource<RealtyLocation>();
    }

    public Task<RealtyLocation> GetMapResultAsync()
    {
        return _taskCompletionSource.Task;
    }

    private async void LoadHtmlToWebView()
    {

        using var stream = await FileSystem.OpenAppPackageFileAsync("map.html");
        using var reader = new StreamReader(stream);

        var contents = reader.ReadToEnd();

        MapWebView.Source = new HtmlWebViewSource
        {
            Html = contents
        };
    }

    private async void OnGetCoordinatesClicked(object sender, EventArgs e)
    {
        string coordsJson = await MapWebView.EvaluateJavaScriptAsync("getLastCoordinates();");

        if (!string.IsNullOrWhiteSpace(coordsJson))
        {
            var jsonString = coordsJson.Replace("\\", "");

            using (JsonDocument doc = JsonDocument.Parse(jsonString))
            {
                RealtyLocation location = new()
                {
                    Latinude = Convert.ToDouble(doc.RootElement.GetProperty("lat").GetString().Replace(".", ",")),
                    Longitude = Convert.ToDouble(doc.RootElement.GetProperty("lng").GetString().Replace(".", ",")),
                    Address = doc.RootElement.GetProperty("address").GetString()
                };
                _lastLocation = location;
                _taskCompletionSource.SetResult(location);
                await Navigation.PopAsync();
            }
        }
    }

    private void OnSearchAddressClicked(object sender, EventArgs e)
    {
        string address = SearchEntry.Text;

        if (!string.IsNullOrWhiteSpace(address))
        {
            address = address.Replace("'", "\\'").Replace("\"", "\\\"");

            MapWebView.EvaluateJavaScriptAsync($"searchAddress('{address}');");
        }
    }

    private void OnWebViewNavigating(object sender, WebNavigatingEventArgs e)
    {
        if (e.Url.StartsWith("app://address"))
        {
            e.Cancel = true;

            var uri = new Uri(e.Url);
            var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
            var address = query.Get("value");

            if (!string.IsNullOrWhiteSpace(address))
            {
                SearchEntry.Text = address;
            }
        }
        else if (e.Url.StartsWith("app://log"))
        {
            e.Cancel = true; 
            var message = e.Url.Substring("app://log?message=".Length);
            Console.WriteLine($"JS log: {Uri.UnescapeDataString(message)}"); 
        }
    }
}

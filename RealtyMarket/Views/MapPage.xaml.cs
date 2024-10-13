using Microsoft.Maui.Platform;
using System.Text.Json;

namespace RealtyMarket.Views;

public partial class MapPage : ContentPage
{
	public MapPage()
	{
		InitializeComponent();

        Shell.SetTabBarIsVisible(this, false);

        string htmlSource = @"
    <!DOCTYPE html>
    <html>
    <head>
        <meta charset='utf-8'>
        <title>Yandex Map</title>
        <script src='https://api-maps.yandex.ru/2.1/?apikey=3225bb0b-e2a7-455f-9c95-ce49d86acbcb&lang=ru_RU' type='text/javascript'></script>
        <script type='text/javascript'>
            ymaps.ready(init);
            var myMap;
            var lastCoords = [0, 0];

            function init(){
                myMap = new ymaps.Map('map', {
                    center: [55.76, 37.64], 
                    zoom: 10,
                    controls: [],
                    theme: 'islands#dark'
                }, {
                    suppressMapOpenBlock: true, 
                    yandexMapDisablePoiInteractivity: true
                }); 

                // Добавляем событие на клик
                myMap.events.add('click', function (e) {
                    lastCoords = e.get('coords');
                    placeMarker(lastCoords);
                });
            }

            function placeMarker(coords) {
                var myPlacemark = new ymaps.Placemark(coords, {}, {
                    preset: 'islands#icon',
                    iconColor: '#0095b6'
                });

                myMap.geoObjects.removeAll(); 
                myMap.geoObjects.add(myPlacemark);
            }

            function getLastCoordinates() {
                return JSON.stringify({
                    lat: lastCoords[0].toFixed(6),
                    lng: lastCoords[1].toFixed(6)
                });
            }
        </script>
    </head>
    <body>
        <div id='map' style='width: 100%; height: 100vh;'></div>
    </body>
    </html>";

        MapWebView.Source = new HtmlWebViewSource
        {
            Html = htmlSource
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
                string lat = doc.RootElement.GetProperty("lat").GetString();
                string lng = doc.RootElement.GetProperty("lng").GetString();

                Console.WriteLine($"Широта: {lat}, Долгота: {lng}");
            }
        }
    }
}

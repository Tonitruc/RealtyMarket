using Microsoft.Maui.Platform;
using RealtyMarket.Controls;
using RealtyMarket.Models;
using RealtyMarket.Models.OtherEntity;
using RealtyMarket.Models.RealtyEntity;
using RealtyMarket.ViewModels;

namespace RealtyMarket.Views
{
    [QueryProperty(nameof(Advertisement), "Advertisement")]
    public partial class AdvertisementPage : ContentPage
    {
        private readonly AdvertisementViewModel _viewModel;

        private Advertisement Advertisement { get; set; }


        public AdvertisementPage(Advertisement advertisement, RegisteredUser user)
        {
            InitializeComponent();

            _viewModel = new AdvertisementViewModel();
            BindingContext = _viewModel;

            _viewModel.IsLoading = true;
            Advertisement = advertisement;
            _viewModel.User = user;
            _viewModel.Advertisement = advertisement;
            LoadAdData();
            _viewModel.LoadPhoto();
            _viewModel.IsLoading = false;

            RealtyLocation location = Advertisement.Realty.Location;

            LoadYandexMap(location.Latinude, location.Longitude);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.IsFavoriteCheck();
        }

        public void LoadAdData()
        {
            AmountFloors.IsVisible = AmountRooms.IsVisible = CeilingHeight.IsVisible =
            ConstructionYear.IsVisible = Conveniences.IsVisible = LivingArea.IsVisible = Newness.IsVisible = false;

            switch (Advertisement.Realty)
            {
                case Flat:
                    GasSystem.IsVisible = Electricity.IsVisible = HouseType.IsVisible = SeweragerSystem.IsVisible =
                    TerritoryConveniences.IsVisible = Water.IsVisible = false;
                    LoadFlatData();
                    break;
                case PrivateHouse:
                    BalconyType.IsVisible = FloorNum.IsVisible = FloorNumber.IsVisible = EntranceRoom.IsVisible =
                    KitchenArea.IsVisible = RepairType.IsVisible = false;
                    LoadHouseData();
                    break;
            }
        }

        private void LoadYandexMap(double latitude, double longitude)
        {
            string lat = latitude.ToString().Replace(",", ".");
            string lon = longitude.ToString().Replace(",", ".");

            string contentHtml = _htmlMapContent = $@"
            <!DOCTYPE html>
            <html lang=""en"">
            <head>
                <meta charset=""UTF-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                <title>Yandex Map</title>
                <script src=""https://api-maps.yandex.ru/2.1/?apikey=3225bb0b-e2a7-455f-9c95-ce49d86acbcb&lang=ru_RU"" type=""text/javascript""></script>
                <style>
                    html, body, #map {{
                        width: 100%; height: 100%; margin: 0; padding: 0;
                    }}
                </style>
            </head>
            <body>
                <div id=""map""></div>
                <script type=""text/javascript"">
                    var belarusBounds = [
                        [51.2568, 23.1783],
                        [56.172, 32.762],
                    ];

                    ymaps.ready(init);
                    function init() {{
                        var myMap = new ymaps.Map('map', {{
                            center: [{lat}, {lon}], // ����� ����� �� �������� �����������
                            zoom: 13,
                            controls: []
                        }}, {{
                            suppressMapOpenBlock: true,
                            yandexMapDisablePoiInteractivity: true,
                            restrictMapArea: belarusBounds
                        }});
                        
                        // ��������� ������� �� ����������
                        var placemark = new ymaps.Placemark([{lat}, {lon}], {{
                            balloonContent: '�����: [{lat}, {lon}]'
                        }});
                        myMap.geoObjects.add(placemark);
                    }}
                </script>
            </body>
            </html>
            ";

            MainThread.BeginInvokeOnMainThread(() =>
            {
                AdAddress.Source = null;
                AdAddress.Source = new HtmlWebViewSource() { Html = contentHtml };
            });
        }

        public void LoadResidentalData()
        {
            var rR = Advertisement.Realty as ResidentialRealty;

            if (rR.AmountFloors != -1)
            {
                AmountFloors.Text = "���������� ������: " + rR.AmountFloors.ToString();
                AmountFloors.IsVisible = true;
            }

            if (rR.AmountRooms != -1)
            {
                AmountRooms.Text = "���������� ������: " + rR.AmountRooms.ToString();
                AmountRooms.IsVisible = true;
            }

            if (!string.IsNullOrEmpty(rR.CeilingHeight))
            {
                CeilingHeight.Text = "������ �������: " + rR.CeilingHeight;
                CeilingHeight.IsVisible = true;
            }

            if (rR.ConstructionYear != -1)
            {
                ConstructionYear.Text = "��� ���������: " + rR.ConstructionYear;
                ConstructionYear.IsVisible = true;
            }

            if (rR.Conveniences.Count != 0)
            {
                Conveniences.Text = "��������: ";
                foreach (var con in rR.Conveniences)
                {
                    Conveniences.Text += con + ", ";
                }
                Conveniences.IsVisible = true;
            }

            if (rR.LivingArea != -1)
            {
                LivingArea.Text = "����� �������: " + rR.LivingArea;
                LivingArea.IsVisible = true;
            }

            if (!string.IsNullOrEmpty(rR.Newness))
            {
                Newness.Text = "���������: " + rR.Newness;
                Newness.IsVisible = true;
            }
        }

        public void LoadFlatData()
        {
            LoadResidentalData();

            var flat = Advertisement.Realty as Flat;

            if (!string.IsNullOrEmpty(flat.BalconyType))
            {
                BalconyType.Text = "��� �������: " + flat.BalconyType;
                BalconyType.IsVisible = true;
            }

            if (flat.Floor != -1)
            {
                FloorNum.Text = "����: " + flat.Floor.ToString();
                FloorNum.IsVisible = true;
            }

            if (flat.FloorNumbers != -1)
            {
                FloorNumber.Text = "��������� ����: " + flat.FloorNumbers.ToString();
                FloorNumber.IsVisible = true;
            }

            if (flat.IsEntranceRoom)
            {
                EntranceRoom.Text = "��������� �������: ����";
                EntranceRoom.IsVisible = true;
            }

            if (flat.KitchenArea != -1)
            {
                KitchenArea.Text = "�������� �������: " + flat.KitchenArea.ToString();
                KitchenArea.IsVisible = true;
            }

            if (!string.IsNullOrEmpty(flat.RepairType))
            {
                RepairType.Text = "������: " + flat.RepairType;
                RepairType.IsVisible = true;
            }
        }

        public void LoadHouseData()
        {
            LoadResidentalData();

            var house = Advertisement.Realty as PrivateHouse;

            if (!string.IsNullOrEmpty(house.GasSystem))
            {
                GasSystem.Text = "���: " + house.GasSystem;
                GasSystem.IsVisible = true;
            }

            Electricity.IsVisible = true;
            Electricity.Text = "�������������: ";
            Electricity.Text += house.HasElectricity ? "����" : "����";

            if (!string.IsNullOrEmpty(house.HouseType))
            {
                HouseType.Text = "��� ����: " + house.HouseType;
                HouseType.IsVisible = true;
            }

            if (!string.IsNullOrEmpty(house.SewerageSystem))
            {
                SeweragerSystem.Text = "�����������: " + house.SewerageSystem;
                SeweragerSystem.IsVisible = true;
            }

            if (house.TerritoryConveniences.Count != 0)
            {
                TerritoryConveniences.Text = "����������: ";
                foreach (var con in house.TerritoryConveniences)
                {
                    TerritoryConveniences.Text += con + ", ";
                }
                TerritoryConveniences.IsVisible = true;
            }

            if (!string.IsNullOrEmpty(house.Water))
            {
                Water.Text = "����: " + house.Water;
                Water.IsVisible = true;
            }
        }

        private string _htmlMapContent;

        private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            await Navigation.PushModalAsync(new FullScreenMapPage(_htmlMapContent));
        }

        private async void ImageButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void AddDeleteAdFavoriteClicked(object sender, EventArgs e)
        {
            var button = sender as GrHeartButton;

            Advertisement ad = Advertisement;

            if (button.IsActive)
            {
                await _viewModel.AddFavorite(ad);
            }
            else
            {
                await _viewModel.DeleteFavorite(ad);
            }
        }
    }
}
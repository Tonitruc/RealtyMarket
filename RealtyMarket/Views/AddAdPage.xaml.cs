using RealtyMarket.Controls;
using RealtyMarket.Converters;
using RealtyMarket.Models;
using RealtyMarket.Models.OtherEntity;
using RealtyMarket.Models.RealtyEntity;
using RealtyMarket.Models.RealtyEntity.Enums;
using RealtyMarket.Repository;
using RealtyMarket.Service;
using RealtyMarket.ViewModels;
using Syncfusion.Maui.DataForm;
using Syncfusion.Maui.Inputs;
using Syncfusion.Maui.ListView;
using System.Collections.ObjectModel;
using System.Net;
using System.Text.RegularExpressions;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace RealtyMarket.Views
{
    public partial class AddAdPage : ContentPage
    {
        private readonly AddAdViewModel _viewModel;

        private RealtyLocation _lastLocation;

        private readonly AdvertisementRepository _advertisementRepository;

        private double _previousScrollY;


        public AddAdPage(AddAdViewModel viewModel,
            AdvertisementRepository advertisementRepository)
        {
            InitializeComponent();

            _advertisementRepository = advertisementRepository; 
            BindingContext = _viewModel = viewModel;

            Shell.SetTabBarIsVisible(this, false);

            CategoryComboBox.SelectedIndex = 0;
            CurrencyComboBox.SelectedIndex = 0;         
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if(_viewModel.NeedClear)
            {
                ClearFields();
                _viewModel.NeedClear = false;
            }

            await _viewModel.GetUserInfo();
            
            if(_viewModel.User == null)
            {
                return;
            }

            EmailEntry.Text = _viewModel.User.Email;
            NameEntry.Text = _viewModel.User.Name;

            if(!string.IsNullOrEmpty(_viewModel.User.Number))
            {
                NumberEntry.Text = _viewModel.User.Number;
            }

            _viewModel.Advertisement = new();
        }

        private void ScrollViewScrolled(object sender, ScrolledEventArgs e)
        {
            double newScrollY = e.ScrollY;

            if (newScrollY > _previousScrollY && newScrollY > 50)
            {
                PageTitle.TranslateTo(0, -PageTitle.Height, 250, Easing.CubicIn);
            }
            else
            {
                PageTitle.TranslateTo(0, 0, 50, Easing.CubicOut);
            }

            _previousScrollY = newScrollY;
        }

        #region AddAdvertisment

        private async void AddAdvertismentButtonClicked(object sender, EventArgs e)
        {
            if (CategoryComboBox.SelectedIndex == 0)
            {
                await DisplayAlert("������", "�������� ���������", "��");
            }

            if (ValidateData())
            {
                return;
            }

            if(CategoryComboBox.SelectedIndex == 1)
            {
                _viewModel.Advertisement.Realty = new Flat();
                await SaveFlatData();
                await _viewModel.PublishAdvertisement();
            }
            else if (CategoryComboBox.SelectedIndex == 2)
            {
                _viewModel.Advertisement.Realty = new PrivateHouse();
                await SavePrivateHouseData();
                await _viewModel.PublishAdvertisement();
            }
        }

        private async Task SaveShareData()
        {
            _viewModel.Advertisement.PublishDate = DateTime.Now;
            _viewModel.Advertisement.UserEmail = EmailEntry.Text;
            _viewModel.Advertisement.SellerName = NameEntry.Text;
            _viewModel.Advertisement.SellerNumber = NumberEntry.Text;
            _viewModel.Advertisement.RealtyCategory = "��������";
            _viewModel.Advertisement.Type = AdvertisementTypesCombo.SelectedText;
            _viewModel.Advertisement.Description = DescriptionEditor.Text;

            if(CurrencyComboBox.SelectedItem.ToString() == "$")
            {
                _viewModel.Advertisement.Cost.Add("USD", Convert.ToDouble(CostEntry.Text));
                double byn = await CurrencyConverter.USDToBYN(_viewModel.Advertisement.Cost["USD"]);
                _viewModel.Advertisement.Cost.Add("BYN", byn);
            }
            else
            {
                _viewModel.Advertisement.Cost.Add("BYN", Convert.ToDouble(CostEntry.Text));
                double usd = await CurrencyConverter.BYNToUSD(_viewModel.Advertisement.Cost["BYN"]);
                _viewModel.Advertisement.Cost.Add("USD", usd);
            }

            _viewModel.Advertisement.Name = AdvertismentNameEntry.Text;
            _viewModel.Advertisement.RealtyCategory = CategoryComboBox.SelectedItem.ToString();
            _viewModel.Advertisement.Realty.Area = Convert.ToDouble(CommonAreaEntry.Text);
            _viewModel.Advertisement.Realty.Location = _lastLocation;
        }

        private void SaveResidentalRealtyData()
        {
            ResidentialRealty rR = _viewModel.Advertisement.Realty as ResidentialRealty;

            if (AmountFloorsCombo.SelectedIndex != -1)
                rR.AmountFloors = Convert.ToInt32(AmountFloorsCombo.SelectedText.Substring(0, 1));
            else
                rR.AmountFloors = -1;

            if (AmountRoomsCombo.SelectedIndex != -1)
                rR.AmountRooms = Convert.ToInt32(AmountRoomsCombo.SelectedText.Substring(0, 1));
            else
                rR.AmountRooms = -1;

            if (CeilingHeight.SelectedIndex != -1) 
                rR.CeilingHeight = CeilingHeight.SelectedText;

            if (!string.IsNullOrEmpty(ConstructedYearEntry.Text))
            {
                rR.ConstructionYear = Convert.ToInt32(ConstructedYearEntry.Text);
            }
            else
            {
                rR.ConstructionYear = -1;
            }

            if (!string.IsNullOrEmpty(LivingAreaEntry.Text))
            {
                rR.LivingArea = Convert.ToInt32(LivingAreaEntry.Text);
            }
            else
            {
                rR.LivingArea = -1;
            }

            rR.Newness = NewnessCombo.SelectedText;
        }

        private async Task SaveFlatData()
        {
            await SaveShareData();
            SaveResidentalRealtyData();
            Flat flat = _viewModel.Advertisement.Realty as Flat;

            flat.BalconyType = BalconyTypeCombo.SelectedText;

            if (!string.IsNullOrEmpty(FlatFloorEntry.Text))
            {
                flat.Floor = Convert.ToInt32(FlatFloorEntry.Text);
            }
            else
            {
                flat.Floor = -1;
            }

            if (!string.IsNullOrEmpty(FloorNumberEntry.Text))
            {
                flat.FloorNumbers = Convert.ToInt32(FloorNumberEntry.Text);
            }
            else
            {
                flat.FloorNumbers = -1;
            }

            flat.IsEntranceRoom = IsEntranceRoomSwitch.IsOn.Value;

            if (!string.IsNullOrEmpty(KitchenAreaEntry.Text))
            {
                flat.KitchenArea = Convert.ToDouble(KitchenAreaEntry.Text);
            }
            else
            {
                flat.KitchenArea = -1;
            }

            flat.RepairType = RepariTypeCombo.SelectedText;

            if(FlatConveniencesComboBox.SelectedItems.Count != 0)
            {
                flat.Conveniences = new List<string>(FlatConveniencesComboBox.SelectedItems);
            }
        }

        private async Task SavePrivateHouseData()
        {
            await SaveShareData();
            SaveResidentalRealtyData();
            PrivateHouse ph = _viewModel.Advertisement.Realty as PrivateHouse;

            ph.HouseType = HouseTypesCombo.SelectedText;
            ph.SewerageSystem = SewerageSystemCombo.SelectedText;
            ph.GasSystem = HouseGasSystem.SelectedText;
            ph.HasElectricity = IsHasElectricityCheck.IsOn.Value;
            ph.TerritoryConveniences = TerritoryConveniences.SelectedItems.ToList();
            ph.Water = HouseWaterTypes.SelectedText;
        }

        private bool ValidateData()
        {
            CommonAreaEntry.IsError = CostEntry.IsError = NumberEntry.IsError = 
                AdvertismentNameEntry.IsError = AdvertisementTypeError.IsVisible = AddressEntry.IsError = false;

            if(AdvertisementTypesCombo.SelectedIndex == -1)
            {
                AdvertisementTypeError.IsVisible = true;
            }

            var text = AddressEntry.Text; //TODO Placeholder not Text
            if (string.IsNullOrEmpty(AddressEntry.Text))
            {
                AddressEntry.IsError = true;
                AddressEntry.ErrorText = "�������� �����";
            }

            if (string.IsNullOrEmpty(AdvertismentNameEntry.Text))
            {
                AdvertismentNameEntry.IsError = true;
                AdvertismentNameEntry.ErrorText = "���� �� ����� ���� ������";
            }

            if (string.IsNullOrEmpty(CommonAreaEntry.Text))
            {
                CommonAreaEntry.IsError = true;
                CommonAreaEntry.ErrorText = "������� ����� �������";
            }

            if (string.IsNullOrEmpty(CostEntry.Text))
            {
                CostEntry.IsError = true;
                CostEntry.ErrorText = "������� ����";
            }

            if (string.IsNullOrEmpty(NumberEntry.Text))
            {
                NumberEntry.IsError = true;
                NumberEntry.ErrorText = "������� ����� ��������";
            }

            return CommonAreaEntry.IsError || CostEntry.IsError || NumberEntry.IsError || AdvertisementTypeError.IsVisible
                 || AdvertismentNameEntry.IsError || AddressEntry.IsError;
        }

        #endregion

        private void IntValidatorEntry(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Contains('.'))
            {
                if(sender is GrEntry entry)
                {   
                    entry.Text = e.OldTextValue;
                }
            }
        }

        private async void SfEffectsView_AnimationCompleted(object sender, EventArgs e)
        {
            MapPage map = new(_lastLocation);

            await Navigation.PushAsync(map);

            RealtyLocation result = await map.GetMapResultAsync();

            _lastLocation = result;
            AddressEntry.Text = result.Address;
        }

        private void RemovePhotoClicked(object sender, EventArgs e)
        {
            if(sender is ImageButton ib)
            {
                _viewModel.RemovePhotoCommand.Execute(ib.BindingContext);
            }
        }

        private void AddPhotoClicked(object sender, EventArgs e)
        {
            _viewModel.AddPhotoCommand.Execute(null);
        }

        public void ClearFields()
        {
            CategoryComboBox.SelectedIndex = 0;
            DescriptionEditor.Text = string.Empty;
            CostEntry.Text = string.Empty;
            AdvertismentNameEntry.Text = string.Empty;
            CommonAreaEntry.Text = string.Empty;
            AddressEntry.Text = "�� �������";

            ConstructedYearEntry.Text = string.Empty;
            LivingAreaEntry.Text = string.Empty;

            FlatFloorEntry.Text = string.Empty;
            FloorNumberEntry.Text = string.Empty;

            IsEntranceRoomSwitch.IsOn = false;
            KitchenAreaEntry.Text = string.Empty;

            AdvertisementTypesCombo.Reset();
            HouseTypesCombo.Reset();
            AmountFloorsCombo.Reset();
            AmountRoomsCombo.Reset();
            FlatToiletCombo.Reset();
            SewerageSystemCombo.Reset();
            HouseGasSystem.Reset();
            CeilingHeight.Reset();
            BalconyTypeCombo.Reset();
            RepariTypeCombo.Reset();
            HouseWaterTypes.Reset();
            NewnessCombo.Reset();
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

            for (int i = 0; i < numericText.Length; i++)
            {
                if (i == 3 || i == 5 || i == 6 || i == 8 || i == 10 || i == 11)
                {
                    formatedText += numericText[i];
                }
                if (i == 4)
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

            if (entry.Text != formatedText)
            {
                entry.Text = formatedText;
                entry.CursorPosition = formatedText.Length;
            }
        }
    }
}
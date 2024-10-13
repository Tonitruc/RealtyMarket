using RealtyMarket.Controls;
using RealtyMarket.Models;
using RealtyMarket.Models.RealtyEntity;
using RealtyMarket.Service;
using RealtyMarket.ViewModels;
using Syncfusion.Maui.DataForm;
using Syncfusion.Maui.Inputs;
using Syncfusion.Maui.ListView;
using System.Collections.ObjectModel;
using System.Net;

namespace RealtyMarket.Views
{
    public partial class AddAdPage : ContentPage
    {
        private readonly AddAdViewModel _viewModel;

        private VerticalStackLayout _currentInputData;

        private double previousScrollY;

        public AddAdPage(AddAdViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = _viewModel = viewModel;

            Shell.SetTabBarIsVisible(this, false);

            CategoryComboBox.SelectedIndex = 0;
            CurrencyComboBox.SelectedIndex = 0;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.GetUserInfo();
            
            EmailEntry.Text = _viewModel.User.Email;
            NameEntry.Text = _viewModel.User.Name;

            if(!string.IsNullOrEmpty(_viewModel.User.Number))
            {
                NumberEntry.Text = _viewModel.User.Number;
            }
        }

        private void ScrollViewScrolled(object sender, ScrolledEventArgs e)
        {
            double newScrollY = e.ScrollY;

            if (newScrollY > previousScrollY)
            {
                PageTitle.TranslateTo(0, -PageTitle.Height, 250, Easing.CubicIn);
            }
            else
            {
                PageTitle.TranslateTo(0, 0, 50, Easing.CubicOut);
            }

            previousScrollY = newScrollY;
        }

        #region AddAdvertisment

        private void AddAdvertismentButtonClicked(object sender, EventArgs e)
        {
            if (!ValidateData())
            {
                return;
            }

            if(CategoryComboBox.SelectedIndex == 1)
            {
                _viewModel.Advertisement.Realty = new Flat();
                SaveFlatData();
                Flat flat = _viewModel.Advertisement.Realty as Flat;
            }
            else if (CategoryComboBox.SelectedIndex == 2)
            {
                _viewModel.Advertisement.Realty = new PrivateHouse();
            }
        }

        private void SaveShareData()
        {
            _viewModel.Advertisement.Name = AdvertismentNameEntry.Text;
            _viewModel.Advertisement.RealtyCategory = CategoryComboBox.SelectedItem.ToString();
            _viewModel.Advertisement.Realty.Area = Convert.ToDouble(CommonAreEntry.Text);
            _viewModel.Advertisement.Realty.Location = new Models.OtherEntity.Location(0.0, 0.0);
        }

        private void SaveResidentalRealtyData()
        {
            ResidentialRealty rR = _viewModel.Advertisement.Realty as ResidentialRealty;

            rR.AmountFloors = Convert.ToInt32(AmountFloorsCombo.SelectedText.Substring(0, 1));

            rR.AmountRooms = Convert.ToInt32(AmountFloorsCombo.SelectedText.Substring(0, 1));

            rR.CeilingHeight = CeilingHeight.SelectedText;

            if(!string.IsNullOrEmpty(ConstructedYearEntry.Text))
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

        private void SaveFlatData()
        {
            SaveShareData();
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

        private bool ValidateData()
        {
            LivingAreaEntry.IsError = CostEntry.IsError = NumberEntry.IsError = false;

            if (string.IsNullOrEmpty(LivingAreaEntry.Text))
            {
                LivingAreaEntry.IsError = true;
                LivingAreaEntry.ErrorText = "Поле не может быть пустым";
            }

            if (string.IsNullOrEmpty(CostEntry.Text))
            {
                CostEntry.IsError = true;
                CostEntry.ErrorText = "Укажите цену";
            }

            if (string.IsNullOrEmpty(NumberEntry.Text))
            {
                NumberEntry.IsError = true;
                NumberEntry.ErrorText = "Укажите номер телефона";
            }

            return LivingAreaEntry.IsError && CostEntry.IsError && NumberEntry.IsError;
        }

        #endregion

        private void IntValidatorEntry(object sender, TextChangedEventArgs e)
        {
            if (!e.NewTextValue.All(Char.IsDigit))
            {
                if(sender is GrEntry entry)
                {
                    entry.Text = e.OldTextValue;
                }
            }
        }
    }
}
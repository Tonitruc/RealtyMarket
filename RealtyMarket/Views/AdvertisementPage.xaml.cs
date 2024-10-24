using RealtyMarket.Models;
using RealtyMarket.ViewModels;

namespace RealtyMarket.Views
{
    [QueryProperty(nameof(Advertisement), "Advertisement")]
    public partial class AdvertisementPage : ContentPage
    {
        private readonly AdvertisementViewModel _viewModel;

        private Advertisement _advertisement;
        public Advertisement Advertisement
        {
            get => _advertisement;
            set
            {
                _advertisement = value;
                _viewModel.Advertisement = value;
                OnPropertyChanged();
            }
        }


        public AdvertisementPage(AdvertisementViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = _viewModel = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        public void LoadAllData()
        {

        }
    }
}
using RealtyMarket.Controls;
using RealtyMarket.Models;
using RealtyMarket.ViewModels;

namespace RealtyMarket.Views
{
    public partial class FavoritesPage : ContentPage
    {
        private readonly FavoriteViewModel _viewModel;

        public FavoritesPage(FavoriteViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = _viewModel = viewModel;
        }

        private bool NeedUpdateAds { get; set; } = true;

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.IsLoading = true;

            if(NeedUpdateAds)
            {
                await _viewModel.GetUser();

                if (_viewModel.User == null)
                {
                    return;
                }

                await _viewModel.GetAdvertisements();
            }
            else
            {
                NeedUpdateAds = true;
            }

            _viewModel.IsLoading = false;
        }

        private async void DeleteFavoriteClicked(object sender, EventArgs e)
        {
            var button = sender as GrHeartButton;

            var ad = (Advertisement)button.Parameter;

            if (_viewModel.User == null)
            {
                await DisplayAlert("Гость", "Зарегестрируйтесь что бы добавить в избранное.", "Ок");
            }

            if (!button.IsActive)
            {
                await _viewModel.DeleteFavorite(ad);

                _viewModel.IsLoading = true;

                _viewModel.Advertisements.Remove(ad);

                _viewModel.IsLoading = false;
            }
        }

        private async void MoreInfoClicked(object sender, EventArgs e)
        {
            var button = (GrButton)sender;

            try
            {
                NeedUpdateAds = false;
                var ad = (Advertisement)button.CommandParameter;
                await Navigation.PushAsync(new AdvertisementPage(ad, _viewModel.User));
            }
            catch (Exception)
            {
                await DisplayAlert("Ошибка", "Объявление не существует", "Ок");
            }

        }
    }
}
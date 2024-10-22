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

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.IsLoading = true;

            await _viewModel.GetUser();

            if(_viewModel.User == null)
            {
                return;
            }

            await _viewModel.GetAdvertisements();

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
    }
}
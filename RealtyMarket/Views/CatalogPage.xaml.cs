using MvvmHelpers;
using RealtyMarket.Controls;
using RealtyMarket.Models;
using RealtyMarket.ViewModels;
using Syncfusion.Maui.Core.Carousel;
using Syncfusion.Maui.ListView;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace RealtyMarket.Views
{
    public partial class CatalogPage : ContentPage
    {
        private readonly CatalogViewModel _viewModel;

        public CatalogPage(CatalogViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = _viewModel = viewModel;

            Shell.SetTitleView(this, null);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.IsLoading = true;

            await _viewModel.GetUser();

            await _viewModel.GetAdvertisements();

            _viewModel.IsLoading = false; 
        }

        private async void AddDeleteAdFavoriteClicked(object sender, EventArgs e)
        {
            var button = sender as GrHeartButton;

            var ad = (Advertisement)button.Parameter;

            if(_viewModel.User == null)
            {
                await DisplayAlert("Гость", "Зарегестрируйтесь что бы добавить в избранное.", "Ок");
            }

            if(button.IsActive)
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
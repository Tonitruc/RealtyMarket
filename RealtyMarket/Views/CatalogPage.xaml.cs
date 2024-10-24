using MvvmHelpers;
using RealtyMarket.Controls;
using RealtyMarket.Models;
using RealtyMarket.ViewModels;
using Syncfusion.Maui.Core.Carousel;
using Syncfusion.Maui.ListView;
using Syncfusion.Maui.PullToRefresh;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace RealtyMarket.Views
{
    public partial class CatalogPage : ContentPage
    {
        private readonly CatalogViewModel _viewModel;

        private FilterPage _filter;

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

            _filter = new FilterPage(); 
        }

        private async void AddDeleteAdFavoriteClicked(object sender, EventArgs e)
        {
            var button = sender as GrHeartButton;

            var ad = (AdvertisementItem)button.Parameter;

            if(button.IsActive)
            {
                await _viewModel.AddFavorite(ad);
            }
            else
            {
                await _viewModel.DeleteFavorite(ad);
            }
        }

        private async void FilterButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(_filter);
        }

        private async void RefreshingCatalog(object sender, EventArgs e)
        {
            PullToRefresh.IsRefreshing = true;
            await Task.Delay(200);
            _viewModel.IsLoading = true;

            await _viewModel.GetAdvertisements();

            PullToRefresh.IsRefreshing = false;
            _viewModel.IsLoading = false;
        }

        private async void MoreInfoClicked(object sender, EventArgs e)
        {
            var button = (GrButton)sender;
            var ad = (Advertisement)button.CommandParameter;

            var navigationParameter = new ShellNavigationQueryParameters
            {
                { "Advertisement", ad }
            };

            await Shell.Current.GoToAsync("//AdvertisementPage", true, navigationParameter);
        }
    }
}
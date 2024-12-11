using CommunityToolkit.Mvvm.ComponentModel;
using AsyncAwaitBestPractices.MVVM;
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

        private RealtyFilter _realtyFilter;

        public CatalogPage(CatalogViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = _viewModel = viewModel;

            Shell.SetTitleView(this, null);
        }

        private bool NeedUpdateAds { get; set; } = true;

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.IsLoading = true;

            if(NeedUpdateAds)
            {
                await _viewModel.GetUser();

                await _viewModel.GetAdvertisements(_realtyFilter);
            }
            else
            {
                NeedUpdateAds = true;
            }

            _viewModel.IsLoading = false;
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
            FilterPage filterPage = new FilterPage(_realtyFilter);

            await Navigation.PushModalAsync(filterPage);

            _realtyFilter = await filterPage.GetFilterResultAsync();
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

            try
            {
                NeedUpdateAds = false;
                var ad = (AdvertisementItem)button.CommandParameter;
                await Navigation.PushAsync(new AdvertisementPage(ad.Advertisement, _viewModel.User));
                await _viewModel.GetUser();
                ad.IsFavorite = _viewModel.IsFavorite(ad.Advertisement);
            }
            catch(Exception)
            {
                await DisplayAlert("ќшибка", "ќбъ€влени€ не существует", "ќк");
            }

        }

        private async void AddressSearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            await _viewModel.SearchAddressCommand(_realtyFilter);
        }

        private async void AddressSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(AddressSearchBar.Text.Length == 0)
            {
                await _viewModel.SearchAddressCommand(_realtyFilter);
            }
        }
    }
}
using MvvmHelpers;
using RealtyMarket.ViewModels;
using Syncfusion.Maui.Core.Carousel;
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

            await _viewModel.GetAdvertisements();
        }
    }
}
using MvvmHelpers;
using System.ComponentModel;

namespace RealtyMarket.Views
{
    public partial class CatalogPage : ContentPage
    {
        public CatalogPage()
        {
            InitializeComponent();

            Shell.SetTitleView(this, null);
        }
    }

    public class CatalogPageVewModel : ObservableObject
    {
        private string _text;

        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);

        }

        public CatalogPageVewModel()
        {

        }
    }
}
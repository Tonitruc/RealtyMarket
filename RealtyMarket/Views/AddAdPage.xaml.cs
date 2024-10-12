using RealtyMarket.Controls;
using RealtyMarket.Models;
using RealtyMarket.Models.RealtyEntity;
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
        AddAdViewModel viewModel;

        private VerticalStackLayout _currentInputData;

        private double previousScrollY;

        public AddAdPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new AddAdViewModel();

            Shell.SetTabBarIsVisible(this, false);

            CategoryComboBox.SelectedIndex = 0;
        }

        private void SfComboBox_SelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
        {
            int selectedIndex = CategoryComboBox.SelectedIndex;
            if(selectedIndex == -1)
            {
                return;
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
    }
}
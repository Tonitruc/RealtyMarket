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
        }

        private void SfComboBox_SelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
        {
            int selectedIndex = CategoryComboBox.SelectedIndex;
            if(selectedIndex == -1)
            {
                return;
            }

            string text = CategoryComboBox.SelectedValue.ToString();
            if (text == "Квартира")
            {
                MainInputLayout.Children.Add(new GrComboLayout() { Title = "Тип сделки", Items = ["Продажа", "Аренда"], Margin = new Thickness(0, 10, 0, 0) });
                MainInputLayout.Children.Add(new GrEntry() { Title = "Адресс", Margin = new Thickness(10, 20, 10, 0)});
                MainInputLayout.Children.Add(new GrEntry() { Title = "Общая площадь, кв.м.", Keyboard = Keyboard.Numeric, Margin = new Thickness(10, 20, 10, 0)});
                MainInputLayout.Children.Add(new GrEntry() { Title = "Жилая площадь, кв.м.", Keyboard = Keyboard.Numeric, Margin = new Thickness(10, 20, 10, 0)});
                MainInputLayout.Children.Add(new GrEntry() { Title = "Площадь кухни, кв.м.", Keyboard = Keyboard.Numeric, Margin = new Thickness(10, 20, 10, 0)});
            }
            else if(text == "Дом")
            {
                 
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
                PageTitle.TranslateTo(0, 0, 250, Easing.CubicOut);
            }

            previousScrollY = newScrollY;
        }
    }
}
using RealtyMarket.Models;
using RealtyMarket.Models.RealtyEntity.Enums;
using RealtyMarket.ViewModels;

namespace RealtyMarket.Views;

public partial class FilterPage : ContentPage
{
    private RealtyFilter _filter;

    private TaskCompletionSource<RealtyFilter> _taskCompletionSource;

	public FilterPage(RealtyFilter state = null)
	{
		InitializeComponent();

        BindingContext = new FilterViewModel();

        this.TranslationX = Application.Current.MainPage.Width;
        CategoryComboBox.SelectedIndex = 0;

        if(state == null)
            _filter = new RealtyFilter();
        else 
            _filter = state;

        _taskCompletionSource = new TaskCompletionSource<RealtyFilter>();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await this.TranslateTo(0, 0, 500, Easing.SinInOut);

        NewSort.IsChecked = CostDescSortSort.IsChecked = CostAscSortSort.IsChecked = false;

        if (_filter.Sort == RealtySort.New)
        {
            NewSort.IsChecked = true;
        }
        else if (_filter.Sort == RealtySort.CostDescending)
        {
            CostDescSortSort.IsChecked = true;
        }
        else if (_filter.Sort == RealtySort.CostAscending)
        {
            CostAscSortSort.IsChecked = true;
        }

        if (!string.IsNullOrEmpty(_filter.Region))
        {
            RegionComboBox.SelectedItem = _filter.Region;
        }
        CategoryComboBox.SelectedItem = _filter.RealtyCategory;
        if (!string.IsNullOrEmpty(_filter.AdType))
        {
            AdvertisementTypesCombo.SelectedText = _filter.AdType;
        }
    }

    private async void ReturnClicked(object sender, EventArgs e)
    {
        await this.TranslateTo(Application.Current.MainPage.Width, 0, 500, Easing.SinInOut);
        _taskCompletionSource.SetResult(_filter);
        await Navigation.PopModalAsync();
    }

    public Task<RealtyFilter> GetFilterResultAsync()
    {
        return _taskCompletionSource.Task;
    }

    private async void AllowClicked(object sender, EventArgs e)
    {
        _filter.AdType = AdvertisementTypesCombo.SelectedText;
        _filter.Region = RegionComboBox.SelectedItem;
        _filter.RealtyCategory = CategoryComboBox.SelectedItem as string;
        
        if(NewSort.IsChecked)
        {
            _filter.Sort = RealtySort.New;
        }
        else if (CostDescSortSort.IsChecked)
        {
            _filter.Sort = RealtySort.CostDescending;
        }
        else if(CostAscSortSort.IsChecked)
        {
            _filter.Sort = RealtySort.CostAscending;
        }

        await this.TranslateTo(Application.Current.MainPage.Width, 0, 500, Easing.SinInOut);
        _taskCompletionSource.SetResult(_filter);
        await Navigation.PopModalAsync();
    }

    private void GrButton_Clicked(object sender, EventArgs e)
    {
        NewSort.IsChecked = CostDescSortSort.IsChecked = CostAscSortSort.IsChecked = false;

        NewSort.IsChecked = true;
        
        _filter.RealtyCategory = CategoryComboBox.SelectedItem as string;
        RegionComboBox.SelectedItem = string.Empty;
        AdvertisementTypesCombo.Reset();
    }
}
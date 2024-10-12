using CommunityToolkit.Maui.Views;
using Mopups.Pages;
using Mopups.Services;
using System.Collections.ObjectModel;

namespace RealtyMarket.Controls;

public partial class GrPopupComboBox : PopupPage
{
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(
        nameof(Title), typeof(string), typeof(GrPopupComboBox), "Title", propertyChanged: (bindable, oldValue, newValue) =>
        {
            var control = (GrPopupComboBox)bindable;
            control.TitleLabel.Text = (string)newValue;
        });

    public static readonly BindableProperty ItemsProperty = BindableProperty.Create(
        nameof(Items), typeof(ObservableCollection<string>), typeof(GrPopupComboBox), new ObservableCollection<string>(), propertyChanged: OnItemsChanged);

    public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(
        nameof(FontSize), typeof(double), typeof(GrPopupComboBox), 18.0, propertyChanged: (bindable, oldValue, newValue) =>
        {
            var control = (GrPopupComboBox)bindable;
            control.TitleLabel.FontSize = (double)newValue;
        });

    private static readonly BindableProperty StrokeColorProperty = BindableProperty.Create(
        nameof(StrokeColor), typeof(Color), typeof(GrPopupComboBox), Colors.Gray, BindingMode.OneWay);

    private static readonly BindableProperty UnSelectedColorProperty = BindableProperty.Create(
        nameof(UnSelectedColor), typeof(Color), typeof(GrPopupComboBox), Colors.White, BindingMode.OneWay);

    private static readonly BindableProperty SelectedColorProperty = BindableProperty.Create(
        nameof(SelectedColor), typeof(Color), typeof(GrPopupComboBox), Colors.Violet, BindingMode.OneWay, propertyChanged: (bindable, oldValue, newValue) =>
        {
            var control = (GrPopupComboBox)bindable;
            control.AllowButton.BackgroundColor = newValue as Color;
        });

    public static readonly BindableProperty IsMultipleProperty = BindableProperty.Create(
        nameof(IsMultiple), typeof(bool), typeof(GrPopupComboBox), true, propertyChanged: (bindable, oldValue, newValue) =>
        {
            var control = (GrPopupComboBox)bindable;
            control.AllowLayout.IsVisible = (bool)newValue;
        });

    public string Title
    {
        get => GetValue(TitleProperty) as string;
        set => SetValue(TitleProperty, value);
    }

    public ObservableCollection<string> Items
    {
        get => (ObservableCollection<string>)GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    public Color StrokeColor
    {
        get => GetValue(StrokeColorProperty) as Color;
        set => SetValue(StrokeColorProperty, value);
    }

    public Color UnSelectedColor
    {
        get => GetValue(UnSelectedColorProperty) as Color;
        set => SetValue(UnSelectedColorProperty, value);
    }
    public Color SelectedColor
    {
        get => GetValue(SelectedColorProperty) as Color;
        set => SetValue(SelectedColorProperty, value);
    }

    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    public bool IsMultiple
    {
        get => (bool)GetValue(IsMultipleProperty);
        set => SetValue(IsMultipleProperty, value);
    }

    public ObservableCollection<string> SelectedItems { get; }

    public string SelectedItem { get; private set; }

    public GrPopupComboBox()
	{
		InitializeComponent();

        SelectedItems = [];

        AllowButton.SetBinding(GrButton.BackgroundColorProperty, new Binding(nameof(SelectedColor), BindingMode.OneWay, source: this));
    }

    private static void OnItemsChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (GrPopupComboBox)bindable;

        if (oldValue is ObservableCollection<string> oldCollection)
        {
            oldCollection.CollectionChanged -= control.OnCollectionChanged;
        }

        if (newValue is ObservableCollection<string> newCollection)
        {
            newCollection.CollectionChanged += control.OnCollectionChanged;
            control.UpdateItems();
        }
    }

    private void OnCollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        UpdateItems();
    }

    private void UpdateItems()
    {
        ContentLayout.Children.Clear();
        for (int i = 0; i < Items.Count; i++)
        {
            var item = Items[i];
            var grid = new Grid() { Margin = new Thickness(0, 20, 0, 0) };
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(2) });

            var label = new Label()
            {
                Text = item,
                HorizontalOptions = LayoutOptions.Start,
                FontSize = FontSize,
                Margin = new Thickness(0, 0, 0, 10)
            };

            var checkBox = new GrCheckBox()
            {
                HorizontalOptions = LayoutOptions.End,
                StrokeColor = StrokeColor,
                SelectedColor = SelectedColor,
                UnSelectedColor = UnSelectedColor,
                Margin = new Thickness(0, 0, 0, 10)
            };

            var boxView = new BoxView()
            {
                BackgroundColor = StrokeColor,
                HeightRequest = 2
            };

            checkBox.SetBinding(CheckBox.HeightRequestProperty, new Binding(nameof(label.HeightProperty), BindingMode.OneWay, source: label));
            label.SetBinding(Label.FontSizeProperty, new Binding(nameof(FontSize), BindingMode.OneWay, source: this));

            grid.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => OnGridTapped(grid, i))
            });

            checkBox.IsEnabled = false;
            checkBox.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => OnGridTapped(grid, i))
            });

            grid.Children.Add(label);
            grid.Children.Add(checkBox);
            grid.Children.Add(boxView);
            grid.SetRow(boxView, 1);
            ContentLayout.Children.Add(grid);
        }
    }

    private void ClearButtonClicked(object sender, EventArgs e)
    {
        SelectedItems.Clear();
        var grCheckBoxList = ContentLayout.Children.OfType<Grid>().ToList();
        foreach (var item in grCheckBoxList)
        {
            var checkBox = item.Children.OfType<GrCheckBox>().First();
            checkBox.IsCheck = false;
        }
    }

    private GrCheckBox _prevSelected;

    private async void OnGridTapped(Grid grid, int index)
    {
        GrCheckBox tip = grid.Children.OfType<GrCheckBox>().First();
        Label label = grid.Children.OfType<Label>().First();

        if (IsMultiple)
        {
            tip.IsCheck = !tip.IsCheck;
            if (tip.IsCheck)
            {
                SelectedItems.Add(label.Text);
            }
            else
            {
                SelectedItems.Remove(label.Text);
            }
        }
        else
        {
            if (_prevSelected != null)
            {
                _prevSelected.IsCheck = false;
            }
            _prevSelected = tip;
            tip.IsCheck = true;
            SelectedItem = label.Text;
            await MopupService.Instance.RemovePageAsync(this);
        }
    }

    private async void AllowButtonClicked(object sender, EventArgs e)
    {
        await MopupService.Instance.RemovePageAsync(this);
    }

    private async void CloseButtonClicked(object sender, EventArgs e)
    {
        await MopupService.Instance.RemovePageAsync(this);
    }
}
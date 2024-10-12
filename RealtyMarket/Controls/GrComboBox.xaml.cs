using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Views;
using Mopups.Hosting;
using Mopups.Services;

namespace RealtyMarket.Controls;

public partial class GrComboBox : ContentView
{
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(
        nameof(Title), typeof(string), typeof(GrComboBox), "Title", propertyChanged: (bindable, oldValue, newValue) =>
        {
            var control = (GrComboBox)bindable;
            control._popupMenu.Title = (string)newValue;
            control.ButtonGridTitle.Text = $"{(string)newValue}: не выбрано";
        });

    public static readonly BindableProperty MarginProperty = BindableProperty.Create(
        nameof(Margin), typeof(Thickness), typeof(GrComboBox), new Thickness(0), propertyChanged: (bindable, oldValue, newValue) =>
        {
            var control = (GrComboBox)bindable;
            control.ButtonGrid.Margin = (Thickness)newValue;
        });

    public static readonly BindableProperty IsMultipleProperty = BindableProperty.Create(
        nameof(IsMultiple), typeof(bool), typeof(GrComboBox), true, propertyChanged: (bindable, oldValue, newValue) =>
        {
            var control = (GrComboBox)bindable;
            control._popupMenu.IsMultiple = (bool)newValue;
        });

    public static readonly BindableProperty HeightRequestProperty = BindableProperty.Create(
        nameof(HeightRequest), typeof(double), typeof(GrComboBox), 40.0, BindingMode.OneWay, propertyChanged: (bindable, oldValue, newValue) =>
        {
            var control = (GrComboBox)bindable;
            control.ButtonGridBorder.HeightRequest = (double)newValue;
        });

    public static readonly BindableProperty WidthRequestProperty = BindableProperty.Create(
        nameof(WidthRequest), typeof(double), typeof(GrComboBox));

    public static readonly BindableProperty ItemsProperty = BindableProperty.Create(
        nameof(Items), typeof(ObservableCollection<string>), typeof(GrComboBox), new ObservableCollection<string>(), propertyChanged: (bindable, oldValue, newValue) =>
        {
            var control = (GrComboBox)bindable;
            control._popupMenu.Items = newValue as ObservableCollection<string>;
        });

    public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(
        nameof(FontSize), typeof(double), typeof(GrComboBox), 18.0, propertyChanged: (bindable, oldValue, newValue) =>
        {
            var control = (GrComboBox)bindable;
            control.ButtonGridTitle.FontSize = (double)newValue;
            control._popupMenu.FontSize = (double)newValue;
        });

    private static readonly BindableProperty StrokeColorProperty = BindableProperty.Create(
        nameof(StrokeColor), typeof(Color), typeof(GrComboBox), Colors.Gray, BindingMode.OneWay, propertyChanged: (bindable, oldValue, newValue) =>
        {
            var control = (GrComboBox)bindable;
            control.ButtonGridBorder.Stroke = (Color)newValue;
            control._popupMenu.StrokeColor = (Color)newValue;
        });

    private static readonly BindableProperty UnSelectedColorProperty = BindableProperty.Create(
        nameof(UnSelectedColor), typeof(Color), typeof(GrComboBox), Colors.White, BindingMode.OneWay, propertyChanged: (bindable, oldValue, newValue) =>
        {
            var control = (GrComboBox)bindable;
            control._popupMenu.UnSelectedColor = (Color)newValue;
        });

    private static readonly BindableProperty SelectedColorProperty = BindableProperty.Create(
        nameof(SelectedColor), typeof(Color), typeof(GrComboBox), Colors.Violet, BindingMode.OneWay, propertyChanged: (bindable, oldValue, newValue) =>
        {
            var control = (GrComboBox)bindable;
            control._popupMenu.SelectedColor = (Color)newValue;
        });

    public string Title
    {
        get => GetValue(TitleProperty) as string;
        set => SetValue(TitleProperty, value);
    }

    public Thickness Margin
    {
        get => (Thickness)GetValue(MarginProperty);
        set => SetValue(MarginProperty, value);
    }

    public bool IsMultiple
    {
        get => (bool)GetValue(IsMultipleProperty);
        set => SetValue(IsMultipleProperty, value);
    }

    public ObservableCollection<string> Items
    {
        get => (ObservableCollection<string>)GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    public double HeightRequest
    {
        get => (double)GetValue(HeightRequestProperty);
        set => SetValue(HeightRequestProperty, value);
    }

    public double WidthRequest
    {
        get => (double)GetValue(WidthRequestProperty);
        set => SetValue(WidthRequestProperty, value);
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

    private readonly GrPopupComboBox _popupMenu;

    public ObservableCollection<string> SelectedItems => _popupMenu.SelectedItems;

    public string SelectedItem => _popupMenu.SelectedItem;

    public GrComboBox()
    {
        InitializeComponent();

        _popupMenu = new GrPopupComboBox();
    }

    private async void ButtonGridBorderTapped(object sender, TappedEventArgs e)
    {
        await MopupService.Instance.PushAsync(_popupMenu);

        _popupMenu.Disappearing += (s, e) =>
        {
            if (IsMultiple)
            {
                ButtonGridTitle.Text = $"{Title}: {SelectedItems.Count} выбрано";
            }
            else
            {
                ButtonGridTitle.Text = $"{Title}: {SelectedItem}";
            }
        };
    }
}
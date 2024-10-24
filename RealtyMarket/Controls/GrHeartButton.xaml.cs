using MvvmHelpers.Commands;
using System.Windows.Input;

namespace RealtyMarket.Controls;

public partial class GrHeartButton : ContentView
{

    public static readonly BindableProperty IsActiveProperty = BindableProperty.Create(
        nameof(IsActive), typeof(bool), typeof(GrHeartButton), false, BindingMode.OneWay, propertyChanging: IsActiveChange);

    public bool IsActive
    {
        get => (bool)GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, value);
    }

    public static readonly BindableProperty ParameterProperty = BindableProperty.Create(
		nameof(Parameter), typeof(object), typeof(GrHeartButton), new object(), BindingMode.TwoWay);

    public object Parameter
    {
        get => GetValue(ParameterProperty);
        set => SetValue(ParameterProperty, value);
    }

    public event EventHandler Clicked;

	public ICommand Command { get; set; }


    public GrHeartButton()
	{
		InitializeComponent();
	}

    private static async void IsActiveChange(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (GrHeartButton)bindable;
        control._isActive = (bool)newValue;
        await control.ChangeButtonStatus();
    }

    private bool _isActive = false;

    public async Task ChangeButtonStatus()
    {
        if (_isActive)
        {
            HeartButton.Scale = 0;
            HeartButton.IsVisible = true;
            await HeartButton.ScaleTo(1, 100, Easing.Linear);
        }
        else
        {
            HeartButton.Scale = 1;
            await HeartButton.ScaleTo(0, 100, Easing.Linear);
            HeartButton.IsVisible = false;
        }
    }

    private void ImageButton_Clicked(object sender, EventArgs e)
    {
        IsActive = !IsActive;

        Clicked?.Invoke(this, EventArgs.Empty);
    }
}
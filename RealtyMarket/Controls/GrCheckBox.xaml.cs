namespace RealtyMarket.Controls;

public partial class GrCheckBox : ContentView
{
	private static readonly BindableProperty SizeProperty = BindableProperty.Create(
		nameof(Size), typeof(double), typeof(GrCheckBox), 25.0, BindingMode.OneWay);

	private static readonly BindableProperty StrokeColorProperty = BindableProperty.Create(
		nameof(StrokeColor), typeof(Color), typeof(GrCheckBox), Colors.Gray, BindingMode.OneWay, propertyChanged: (bindable, oldValue, newValue) =>
		{
			var control = (GrCheckBox)bindable;
			control.TipBorder.Stroke = (Color)newValue;
		});

    private static readonly BindableProperty UnSelectedColorProperty = BindableProperty.Create(
		nameof(UnSelectedColor), typeof(Color), typeof(GrCheckBox), Colors.White, BindingMode.OneWay, propertyChanged: (bindable, oldValue, newValue) =>
		{
		    var control = (GrCheckBox)bindable;
		    control.TipBorder.BackgroundColor = (Color)newValue;
		});

	private static readonly BindableProperty SelectedColorProperty = BindableProperty.Create(
		nameof(SelectedColor), typeof(Color), typeof(GrCheckBox), Colors.Violet, BindingMode.OneWay);

    public double Size
	{
		get => (double)GetValue(SizeProperty);
		set => SetValue(SizeProperty, value);
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

    private bool _isCheck = false;

	public bool IsCheck
	{
		get => _isCheck;
		set 
		{
			_isCheck = value;
            TipImage.Source = _isCheck ? "checkbox_icon.png" : null;

            if (IsCheck)
            {
                TipTrueAnimation();
            }
            else
            {
                TipFalseAnimation();
            }
        }
	}

	public GrCheckBox()
	{
		InitializeComponent();

		TipBorder.SetBinding(Border.HeightRequestProperty, new Binding(nameof(Size), BindingMode.OneWay, source: this));
		TipBorder.SetBinding(Border.WidthRequestProperty, new Binding(nameof(Size), BindingMode.OneWay, source: this));
	}

    private void BorderTapped(object sender, TappedEventArgs e)
    {
		_isCheck = !_isCheck;
		TipImage.Source = _isCheck ? "checkbox_icon.png" : null;

		if(IsCheck)
		{
			TipTrueAnimation();
        }
		else
		{
			TipFalseAnimation();
        }
    }

	private void TipTrueAnimation()
	{
        TipBorder.BackgroundColor = SelectedColor;
        TipImage.Scale = 0;

        var scaleDownBorderAnimation = new Animation((v) => { TipBorder.Scale = v; }, 1, 0.8, Easing.Linear); 
        var scaleUpBorderAnimation = new Animation((v) => { TipBorder.Scale = v; }, 0.8, 1, Easing.Linear);
		var scaleTipImage = new Animation((v) => { TipImage.Scale = v; }, 0, 1, Easing.Linear);

        var parentAnimation = new Animation();
		parentAnimation.Add(0, 0.5, scaleDownBorderAnimation);
		parentAnimation.Add(0, 1, scaleUpBorderAnimation);
		parentAnimation.Add(0, 1, scaleTipImage);

		parentAnimation.Commit(this, "ScaleTipTrue", 16, 200, Easing.Linear);
    }

    private void TipFalseAnimation()
	{
		TipBorder.BackgroundColor= UnSelectedColor;
		TipBorder.StrokeThickness = TipBorder.Height / 2;
		TipImage.Scale = 0;

        var scaleDownBorderAnimation = new Animation((v) => { TipBorder.Scale = v; }, 1, 0.8, Easing.Linear);
        var scaleUpBorderAnimation = new Animation((v) => { TipBorder.Scale = v; }, 0.8, 1, Easing.Linear);
        var scaleBorderThickness = new Animation((v) => { TipBorder.StrokeThickness = v; }, TipBorder.StrokeThickness, 2, Easing.Linear);

		var parentAnimation = new Animation();
        parentAnimation.Add(0, 0.5, scaleDownBorderAnimation);
        parentAnimation.Add(0, 1, scaleUpBorderAnimation);
        parentAnimation.Add(0, 1, scaleBorderThickness);

        parentAnimation.Commit(this, "ScaleTipFalse", 16, 200, Easing.Linear);
    }
}
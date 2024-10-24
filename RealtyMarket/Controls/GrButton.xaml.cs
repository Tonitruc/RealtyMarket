using RealtyMarket.Converters;
using Syncfusion.Maui.Core;
using System.Windows.Input;

namespace RealtyMarket.Controls
{
    public partial class GrButton : ContentView
    {
        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            nameof(Text), typeof(string), typeof(GrButton), "Button", BindingMode.OneWay, propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (GrButton)bindable;
                control.ButtonLabel.Text = newValue as string;
            });

        public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(
            nameof(FontFamily), typeof(string), typeof(GrButton), "NexaTrialBold", BindingMode.OneWay, propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (GrButton)bindable;
                control.ButtonLabel.FontFamily = newValue as string;
            });

        public static readonly BindableProperty LoadingTextProperty = BindableProperty.Create(
            nameof(LoadingText), typeof(string), typeof(GrButton), "Loading...", BindingMode.OneWay, propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (GrButton)bindable;
                control.LoadingLabel.Text = newValue as string;
            });

        public static readonly BindableProperty HeightRequestProperty = BindableProperty.Create(
            nameof(HeightRequest), typeof(double), typeof(GrButton), 50.0, BindingMode.OneWay, propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (GrButton)bindable;
                control.ButtonFrame.HeightRequest = (double)newValue;
            });

        public static readonly BindableProperty WidthRequestProperty = BindableProperty.Create(
            nameof(WidthRequest), typeof(double), typeof(GrButton), 100.0, BindingMode.OneWay, propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (GrButton)bindable;
                control.ButtonFrame.WidthRequest = (double)newValue;
            });

        public static readonly BindableProperty HorizontalOptionsProperty = BindableProperty.Create(
            nameof(HorizontalOptions), typeof(LayoutOptions), typeof(GrButton), LayoutOptions.Start, BindingMode.OneWay, propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (GrButton)bindable;
                control.MainLayout.HorizontalOptions = (LayoutOptions)newValue;
            });

        public static readonly BindableProperty VerticalOptionsProperty = BindableProperty.Create(
            nameof(VerticalOptions), typeof(LayoutOptions), typeof(GrButton), LayoutOptions.Start, BindingMode.OneWay, propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (GrButton)bindable;
                control.MainLayout.VerticalOptions = (LayoutOptions)newValue;
            });

        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(
            nameof(CornerRadius), typeof(float), typeof(GrButton), 5.0f, BindingMode.OneWay, propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (GrButton)bindable;
                control.ButtonFrame.CornerRadius = (float)newValue;
            });

        public static readonly BindableProperty EffectColorProperty = BindableProperty.Create(
            nameof(EffectColor), typeof(Color), typeof(GrButton), Colors.DarkGray, BindingMode.OneWay, propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (GrButton)bindable;
                control.WaveEffect.BackgroundColor = newValue as Color;
            });

        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
            nameof(TextColor), typeof(Color), typeof(GrButton), Colors.Black, BindingMode.OneWay, propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (GrButton)bindable;
                control.ButtonLabel.TextColor = newValue as Color;
            });

        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(
            nameof(FontSize), typeof(double), typeof(GrButton), 14.0, BindingMode.OneWay, propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (GrButton)bindable;
                control.ButtonLabel.FontSize = (double)newValue;
            });

        public static readonly BindableProperty IsEnabledProperty = BindableProperty.Create(
            nameof(IsEnabled), typeof(bool), typeof(GrButton), true, BindingMode.OneWay, propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (GrButton)bindable;
                control.DarkEffect.BackgroundColor = control.DisableBackgroundColor;
                control.DarkEffect.IsVisible = !(bool)newValue;
                if((bool)newValue)
                {
                    control.ButtonFrame.Opacity = 1;
                    control.ButtonLabel.TextColor = control.TextColor;
                }
                else
                {
                    control.ButtonFrame.Opacity = 0.5;
                    control.ButtonLabel.TextColor = control.DisableTextColor;
                }
            });

        public static readonly BindableProperty BackgroundColorProperty = BindableProperty.Create(
            nameof(BackgroundColor), typeof(Color), typeof(GrButton), Colors.LightGray, BindingMode.OneWay, propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (GrButton)bindable;
                control.ButtonFrame.BackgroundColor = newValue as Color;
            });

        public static readonly BindableProperty IsBusyProperty = BindableProperty.Create(
            nameof(IsBusy), typeof(bool), typeof(GrButton), false, BindingMode.OneWay);

        public static readonly BindableProperty CommandProperty = BindableProperty.Create(
            nameof(Command),
            typeof(ICommand),
            typeof(GrButton),
            null);


        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
            nameof(CommandParameter),
            typeof(object),
            typeof(GrButton),
            null);

        public static readonly BindableProperty PaddingProperty = BindableProperty.Create(
            nameof(Padding), typeof(Thickness), typeof(GrButton), new Thickness(5), BindingMode.OneWay, propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (GrButton)bindable;
                control.ButtonFrame.Padding = (Thickness)newValue;
            });

        public static readonly BindableProperty MarginProperty = BindableProperty.Create(
            nameof(Margin), typeof(Thickness), typeof(GrButton), new Thickness(0), BindingMode.OneWay, propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (GrButton)bindable;
                control.MainLayout.Margin = (Thickness)newValue;
            });

        public Thickness Margin
        {
            get => (Thickness)GetValue(MarginProperty);
            set => SetValue(MarginProperty, value);
        }

        public static readonly BindableProperty TextAlligmentProperty = BindableProperty.Create(
            nameof(TextAlligment), typeof(TextAlignment), typeof(GrButton), TextAlignment.Center, BindingMode.OneWay, propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (GrButton)bindable;
                control.ButtonLabel.HorizontalTextAlignment = (TextAlignment)newValue;
            });

        public TextAlignment TextAlligment
        {
            get => (TextAlignment)GetValue(TextAlligmentProperty);
            set => SetValue(TextAlligmentProperty, value);
        }

        public string Text
        {
            get => GetValue(TextProperty) as string;
            set => SetValue(TextProperty, value);
        }

        public string FontFamily
        {
            get => GetValue(FontFamilyProperty) as string;
            set => SetValue(FontFamilyProperty, value);
        }

        public string LoadingText
        {
            get => GetValue(LoadingTextProperty) as string;
            set => SetValue(LoadingTextProperty, value);
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

        public double Width
        {
            get => ButtonFrame.Width;
        }

        public LayoutOptions HorizontalOptions
        {
            get => (LayoutOptions)GetValue(HorizontalOptionsProperty);
            set => SetValue(HorizontalOptionsProperty, value);
        }

        public LayoutOptions VerticalOptions
        {
            get => (LayoutOptions)GetValue(VerticalOptionsProperty);
            set => SetValue(VerticalOptionsProperty, value);
        }

        public float CornerRadius
        {
            get => (float)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public Color EffectColor
        {
            get => GetValue(EffectColorProperty) as Color;
            set => SetValue(EffectColorProperty, value);
        }

        public Color TextColor
        {
            get => GetValue(TextColorProperty) as Color;
            set => SetValue(TextColorProperty, value);
        }

        private Color _disableBackgroundColor = Colors.LightGray;

        public Color DisableBackgroundColor
        {
            get => _disableBackgroundColor;
            set => _disableBackgroundColor = value;
        }

        private Color _disableTextColor = Colors.LightGray;

        public Color DisableTextColor
        {
            get => _disableTextColor;
            set => _disableTextColor = value;
        }

        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        public bool IsEnabled
        {
            get => (bool)GetValue(IsEnabledProperty);
            set => SetValue(IsEnabledProperty, value);
        }

        public Color BackgroundColor
        {
            get => GetValue(BackgroundColorProperty) as Color;
            set => SetValue(BackgroundColorProperty, value);
        }

        public bool IsBusy
        {
            get => (bool)GetValue(IsBusyProperty);
            set => SetValue(IsBusyProperty, value);
        }

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public Thickness Padding
        {
            get => (Thickness)GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }

        public event EventHandler Clicked;

        public GrButton()
        {
            InitializeComponent();

            LoadingLabel.SetBinding(Label.IsVisibleProperty, new Binding(nameof(IsBusy), BindingMode.OneWay, source: this));
            LoadingIndicator.SetBinding(SfBusyIndicator.IsVisibleProperty, new Binding(nameof(IsBusy), BindingMode.OneWay, source: this));

            var inverseBooleanConverter = new InverseBooleanConverter();
            ButtonLabel.SetBinding(Label.IsVisibleProperty, new Binding(nameof(IsBusy), BindingMode.OneWay, source: this, converter: inverseBooleanConverter));
        }

        private void TappedAnimation()
        {
            WaveEffect.WidthRequest = 0;
            WaveEffect.HeightRequest = 0;

            var waveXAnimation = new Animation(v =>
            {
                WaveEffect.WidthRequest = v;
            }, 0, ButtonFrame.Width);

            var waveYnimation = new Animation(v =>
            {
                WaveEffect.HeightRequest = v;
            }, 0, ButtonFrame.Height);

            var fadeOutAnimation = new Animation(v => WaveEffect.Opacity = v, 0.5, 0);

            var fadeOutAnimation2 = new Animation(v => WaveEffect.Opacity = v, 0, 0.5);

            var parentAnimation = new Animation();
            parentAnimation.Add(0, 0.7, waveXAnimation);
            parentAnimation.Add(0, 0.35, waveYnimation);
            parentAnimation.Add(0, 0.8, fadeOutAnimation2);
            parentAnimation.Add(0.8, 1, fadeOutAnimation);

            parentAnimation.Commit(this, "WaveEffectAnimation", 16, 200, Easing.Linear);
        }

        private void ButtonFrame_LongPressed(object sender, MR.Gestures.LongPressEventArgs e)
        {
            if (!IsEnabled || IsBusy)
            {
                return;
            }

            ButtonFrame.BackgroundColor = BackgroundColor;
        }

        private void ButtonFrame_LongPressing(object sender, MR.Gestures.LongPressEventArgs e)
        {
            if (!IsEnabled || IsBusy)
            {
                return;
            }

            Color darkColor = new Color(BackgroundColor.Red - 10, BackgroundColor.Green - 5, BackgroundColor.Blue);
            ButtonFrame.BackgroundColor = darkColor;
            TappedAnimation();
        }

        private void ButtonFrame_Tapping(object sender, MR.Gestures.TapEventArgs e)
        {
            if (!IsEnabled || IsBusy)
            {
                return;
            }

            Color darkColor = new Color(BackgroundColor.Red - 10, BackgroundColor.Green - 5, BackgroundColor.Blue);
            Color prevColor = BackgroundColor;
            ButtonFrame.BackgroundColor = darkColor;
            TappedAnimation();
            ButtonFrame.BackgroundColor = prevColor;

            if (Command?.CanExecute(CommandParameter) == true)
            {
                Command.Execute(CommandParameter);
            }
            Clicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
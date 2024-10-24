namespace RealtyMarket.Controls
{
    public partial class GrEntry : ContentView
    {
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(
            nameof(Title), typeof(string), typeof(GrEntry), "Title", propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (GrEntry)bindable;
                control.EntryName.Text = newValue as string;
            });

        public string Title
        {
            get => GetValue(TitleProperty) as string;
            set => SetValue(TitleProperty, value);
        }

        public static readonly BindableProperty MessageProperty = BindableProperty.Create(
            nameof(Message), typeof(string), typeof(GrEntry), "", propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (GrEntry)bindable;
                control.MessageLabel.Text = newValue as string;
            });

        public string Message
        {
            get => GetValue(MessageProperty) as string;
            set => SetValue(MessageProperty, value);
        }

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            nameof(Text), typeof(string), typeof(GrEntry), "", BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (GrEntry)bindable;
                control.EntryLine.TextChanged -= control.OnEntryTextChanged;
                control.EntryLine.Text = newValue as string;
                control.EntryLine.TextChanged += control.OnEntryTextChanged;
                if((string)newValue == string.Empty)
                {
                    control.OnEntryUnfocused(control.EntryLine, null);
                    control._entryLength = 0;
                }
                else
                {
                    control.OnEntryFocused(control.EntryLine, null);
                }
                control.IsFocused = false;
            });

        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
            nameof(TextColor), typeof(Color), typeof(GrEntry), Colors.Black, BindingMode.OneWay, propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (GrEntry)bindable;
                control.EntryLine.TextColor = newValue as Color;
            });

        public static readonly BindableProperty ColorProperty = BindableProperty.Create(
            nameof(Color), typeof(Color), typeof(GrEntry), Colors.Black, BindingMode.OneWay, propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (GrEntry)bindable;
                control.Underline.BackgroundColor = newValue as Color;
                control.MaxLengthLabel.TextColor = newValue as Color;
                control.MessageLabel.TextColor = newValue as Color;
            });

        public static readonly BindableProperty FocusedColorProperty = BindableProperty.Create(
            nameof(FocusedColor), typeof(Color), typeof(GrEntry), Colors.Black, BindingMode.OneWay, propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (GrEntry)bindable;
                control.OnPropertyChanged(nameof(CurrentColor));
            });

        public string Text
        {
            get => GetValue(TextProperty) as string;
            set => SetValue(TextProperty, value);
        }

        private bool _isPasswordVisible = false;

        public static readonly BindableProperty IsPasswordProperty = BindableProperty.Create(
            nameof(IsPassword), typeof(bool), typeof(GrEntry), false, propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (GrEntry)bindable;
                control.EntryLine.IsPassword = (bool)newValue;
                control.UpdateGridStructure();
            });

        public bool IsPassword
        {
            get => (bool)GetValue(IsPasswordProperty);
            set => SetValue(IsPasswordProperty, value);
        }

        private bool _isFocused = false;

        private bool _isError = false;

        public static readonly BindableProperty IsErrorProperty = BindableProperty.Create(
            nameof(IsError), typeof(bool), typeof(GrEntry), false, propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (GrEntry)bindable;
                control._isError = (bool)newValue;
                if(!string.IsNullOrEmpty(control.Message))
                {
                    control.MessageLabel.IsVisible = false;
                }
                else
                {
                    control.MessageLabel.IsVisible = true;
                }
                control.OnPropertyChanged(nameof(CurrentColor));
            });

        protected new bool IsFocused
        {
            get => _isFocused;
            set
            {
                _isFocused = value;
                OnPropertyChanged(nameof(CurrentColor));
            }
        }

        public bool IsError
        {
            get => (bool)GetValue(IsErrorProperty);
            set
            {
                SetValue(IsErrorProperty, value);
                OnPropertyChanged(nameof(CurrentColor));
            }
        }

        public Color FocusedColor
        {
            get => GetValue(FocusedColorProperty) as Color;
            set => SetValue(FocusedColorProperty, value);
        }

        public Color TextColor
        {
            get => GetValue(TextColorProperty) as Color;
            set => SetValue(TextColorProperty, value);
        }

        public Color Color
        {
            get => GetValue(ColorProperty) as Color;
            set => SetValue(ColorProperty, value);
        }

        public Color CurrentColor
        {
            get
            {
                if (_isError)
                    return Colors.Red;

                if (!_isError && IsFocused)
                    return FocusedColor;

                return Color;
            }
        }

        public static readonly BindableProperty ErrorTextProperty = BindableProperty.Create(
            nameof(ErrorText), typeof(string), typeof(GrEntry), "Error", propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (GrEntry)bindable;
                control.IsErrorMessage.Text = newValue as string;
            });

        public string ErrorText
        {
            get => GetValue(ErrorTextProperty) as string;
            set => SetValue(ErrorTextProperty, value);
        }

        public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create(
            nameof(MaxLength), typeof(int), typeof(GrEntry), int.MaxValue, propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (GrEntry)bindable;
                if ((int)newValue != int.MaxValue)
                {
                    if (control.EntryLength > control.MaxLength)
                    {
                        control.Text = control.Text.Substring((int)newValue - 1);
                    }
                    control.MaxLengthLabel.Text = $"{control.EntryLength}/{(int)newValue}";
                    control.MaxLengthLabel.IsVisible = true;
                }
                else
                {
                    control.MaxLengthLabel.IsVisible = false;
                }
                control.EntryLine.MaxLength = (int)newValue;
            });

        public int MaxLength
        {
            get => (int)GetValue(MaxLengthProperty);
            set => SetValue(MaxLengthProperty, value);
        }

        private int _entryLength;

        public int EntryLength => _entryLength;

        public static readonly BindableProperty KeyboardProperty = BindableProperty.Create(
            nameof(Keyboard), typeof(Keyboard), typeof(GrEntry), Keyboard.Default, propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (GrEntry)bindable;
                control.EntryLine.Keyboard = (Keyboard)newValue;
            });

        public Keyboard Keyboard
        {
            get => (Keyboard)GetValue(KeyboardProperty);
            set => SetValue(KeyboardProperty, value);
        }

        public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(
            nameof(ImageSource), typeof(string), typeof(GrEntry), "", propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (GrEntry)bindable;
                control.UpdateGridStructure();
            });

        public string ImageSource
        {
            get => GetValue(ImageSourceProperty) as string;
            set => SetValue(ImageSourceProperty, value);
        }

        public new event EventHandler Focused;
        public new event EventHandler Unfocused;
        public event EventHandler<TextChangedEventArgs> TextChanged;

        public GrEntry()
        {
            InitializeComponent();
            EntryLine.Focused += OnEntryFocused;
            EntryLine.Unfocused += OnEntryUnfocused;
            EntryLine.TextChanged += OnEntryTextChanged;

            EntryLine.SetBinding(Entry.TextColorProperty, new Binding(nameof(TextProperty), BindingMode.TwoWay, source: this));
            //EntryImage.SetBinding(Image.HeightProperty, new Binding(nameof(EntryLine.HeightProperty), BindingMode.TwoWay, source: EntryLine));
            EntryName.SetBinding(Label.TextColorProperty, new Binding(nameof(CurrentColor), BindingMode.OneWay, source: this));
            Underline.SetBinding(BoxView.ColorProperty, new Binding(nameof(CurrentColor), BindingMode.OneWay, source: this));
            PasswordHideButton.SetBinding(ImageButton.IsVisibleProperty, new Binding(nameof(IsPassword), BindingMode.TwoWay, source: this));
            IsErrorMessage.SetBinding(Label.IsVisibleProperty, new Binding(nameof(IsError), BindingMode.TwoWay, source: this));
            MaxLengthLabel.SetBinding(Label.TextProperty, new Binding(nameof(MaxLength), BindingMode.OneWay, source: this));
            EntryLine.SetBinding(Entry.KeyboardProperty, new Binding(nameof(Keyboard), source: this));
        }

        private static Task<bool> AnimateFontSize(Label label, double startSize, double endSize)
        {
            var tcs = new TaskCompletionSource<bool>();

            var animation = new Animation(v => label.FontSize = v, startSize, endSize);
            animation.Commit(label, "FontSizeAnimation", 16, 100, Easing.Linear, finished: (v, c) => tcs.SetResult(true));

            return tcs.Task;
        }

        private async void OnEntryFocused(object sender, FocusEventArgs e)
        {
            IsFocused = true;

            var translateTask = EntryName.TranslateTo(0, -15, 100, Easing.Linear);
            var fontAnimation = AnimateFontSize(EntryName, EntryName.FontSize, 12);

            await Task.WhenAll(translateTask, fontAnimation);

            Focused?.Invoke(this, EventArgs.Empty);
        }

        private async void OnEntryUnfocused(object sender, FocusEventArgs e)
        {
            IsFocused = false;


            if (string.IsNullOrWhiteSpace(EntryLine.Text))
            {
                var translateTask = EntryName.TranslateTo(0, 0, 100, Easing.Linear);
                var fontAnimation = AnimateFontSize(EntryName, EntryName.FontSize, 16);

                await Task.WhenAll(translateTask, fontAnimation);
            }

            Unfocused?.Invoke(this, EventArgs.Empty);
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var oldText = Text;
            Text = e.NewTextValue;
            _entryLength = Text.Length;
            MaxLengthLabel.Text = $"{_entryLength}/{MaxLength}";
            TextChanged?.Invoke(this, new(oldText, EntryLine.Text));
        }

        private void UpdateGridStructure()
        {
            ParentGrid.ColumnDefinitions.Clear();

            if (IsPassword)
            {  
                if(!string.IsNullOrEmpty(ImageSource))
                {
                    ParentGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = EntryImage.HeightRequest + 5 });
                    EntryImage.IsVisible = true;
                }
                ParentGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                ParentGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = 40 });

                PasswordHideButton.IsVisible = true;

                if (string.IsNullOrEmpty(ImageSource))
                {
                    Grid.SetColumnSpan(Underline, 2);
                    Grid.SetColumn(PasswordHideButton, 1);
                }
                else
                {
                    EntryImage.Source = ImageSource;
                    Grid.SetColumn(EntryLine, 1);
                    Grid.SetColumn(Underline, 1);
                    Grid.SetColumnSpan(Underline, 2);
                    Grid.SetColumn(PasswordHideButton, 2);
                }
            }
            else if(!string.IsNullOrEmpty(ImageSource))
            {
                EntryImage.IsVisible = true;
                ParentGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = EntryImage.HeightRequest + 5 });
                ParentGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                EntryImage.Source = ImageSource;
                ParentGrid.SetColumn(EntryName, 1);
                ParentGrid.SetColumn(EntryLine, 1);
                ParentGrid.SetColumnSpan(MessagesGrid, 2);
                ParentGrid.SetColumn(Underline, 1);
            }
            else
            {
                EntryImage.IsVisible = false;
                ParentGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                PasswordHideButton.IsVisible = false;
            }
        }

        private void OnTogglePasswordVisibilityTapped(object sender, EventArgs e)
        {
            _isPasswordVisible = !_isPasswordVisible;
            EntryLine.IsPassword = !_isPasswordVisible;
            PasswordHideButton.Source = _isPasswordVisible ? "show_password.png" : "hide_password.png";
        }
    }
}
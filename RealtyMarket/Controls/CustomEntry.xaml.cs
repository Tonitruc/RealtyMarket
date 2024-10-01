namespace RealtyMarket.Controls
{
    public partial class CustomEntry : ContentView
    {
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(
            nameof(Title), typeof(string), typeof(CustomEntry), "Title", propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (CustomEntry)bindable;
                control.EntryName.Text = newValue as string;
            });

        public string Title
        {
            get => GetValue(TitleProperty) as string;
            set => SetValue(TitleProperty, value);
        }

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            nameof(Text), typeof(string), typeof(CustomEntry), "", propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (CustomEntry)bindable;
                control.EntryLine.Text = newValue as string;
            });

        public string Text
        {
            get => GetValue(TextProperty) as string;
            set => SetValue(TextProperty, value);
        }

        private bool _isPasswordVisible = false;

        public static readonly BindableProperty IsPasswordProperty = BindableProperty.Create(
            nameof(IsPassword), typeof(bool), typeof(CustomEntry), false, propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (CustomEntry)bindable;
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
            nameof(IsError), typeof(bool), typeof(CustomEntry), false, propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (CustomEntry)bindable;
                control._isError = (bool)newValue;
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

        public Color CurrentColor
        {
            get
            {
                if (_isError)
                    return Colors.Red;

                if(!_isError && IsFocused)
                    return Colors.MediumPurple;

                return Colors.Black;
            }
        }

        public static readonly BindableProperty ErrorTextProperty = BindableProperty.Create(
            nameof(ErrorText), typeof(string), typeof(CustomEntry), "Error", propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (CustomEntry)bindable;
                control.IsErrorMessage.Text = newValue as string;
            });

        public string ErrorText
        {
            get => GetValue(ErrorTextProperty) as string;
            set => SetValue(ErrorTextProperty, value);
        }

        public new event EventHandler Focused;
        public new event EventHandler Unfocused;

        public delegate void TextChangedEventHandler(object sender, TextChangedEventArgs e);
        public event TextChangedEventHandler TextChanged;

        public CustomEntry()
        {
            InitializeComponent();
            EntryLine.Focused += OnEntryFocused;
            EntryLine.Unfocused += OnEntryUnfocused;
            EntryLine.TextChanged += OnEntryTextChanged;

            BindingContext = this;
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
            Text = e.NewTextValue;

            TextChanged?.Invoke(this, null);
        }

        private void UpdateGridStructure()
        {
            ParentGrid.ColumnDefinitions.Clear();

            if (IsPassword)
            {
                ParentGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                ParentGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = 40 });

                PasswordHideButton.IsVisible = true;

                Grid.SetColumn(PasswordHideButton, 1);
            }
            else
            {
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
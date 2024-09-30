namespace RealtyMarket
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
            get => GetValue(TitleProperty) as string ?? string.Empty;
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
            get => GetValue(TextProperty) as string ?? string.Empty;
            set => SetValue(TextProperty, value);
        }

        public static readonly BindableProperty IsEmptyValidatorProperty = BindableProperty.Create(
            nameof(IsEmptyValidator), typeof(bool), typeof(CustomEntry), false);

        public bool IsEmptyValidator
        {
            get => (bool)GetValue(IsEmptyValidatorProperty);
            set => SetValue(IsEmptyValidatorProperty, value);
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


        public new event EventHandler Focused;
        public new event EventHandler Unfocused;
        public event EventHandler TextChanged;

        public CustomEntry()
        {
            InitializeComponent();
            EntryLine.Focused += OnEntryFocused;
            EntryLine.Unfocused += OnEntryUnfocused;
            EntryLine.TextChanged += OnEntryTextChanged;
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
            var translateTask = EntryName.TranslateTo(0, -15, 100, Easing.Linear);
            var fontAnimation = AnimateFontSize(EntryName, EntryName.FontSize, 12);
            EntryName.TextColor = Colors.Blue;

            await Task.WhenAll(translateTask, fontAnimation);

            if (IsEmptyValidator)
            {
                if (string.IsNullOrWhiteSpace(EntryLine.Text))
                {
                    IsEmptyErrorMessage.IsVisible = true;
                    EntryName.TextColor = Colors.Red;
                }
                else
                {
                    EntryName.TextColor = Colors.Black;
                }
            }

            Focused?.Invoke(this, EventArgs.Empty);
        }

        private async void OnEntryUnfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EntryLine.Text))
            {
                var translateTask = EntryName.TranslateTo(0, 0, 100, Easing.Linear);
                var fontAnimation = AnimateFontSize(EntryName, EntryName.FontSize, 16);
                EntryName.TextColor = Colors.Black;

                await Task.WhenAll(translateTask, fontAnimation);

                if (IsEmptyValidator)
                {
                    if (string.IsNullOrWhiteSpace(EntryLine.Text))
                    {
                        IsEmptyErrorMessage.IsVisible = true;
                    }
                    EntryName.TextColor = Colors.Black;
                }
            }
            else
            {
                IsEmptyErrorMessage.IsVisible = false;
                EntryName.TextColor = Colors.Black;
            }

            Unfocused?.Invoke(this, EventArgs.Empty);
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(EntryLine.Text))
            {
                IsEmptyErrorMessage.IsVisible = false;
                EntryName.TextColor = Colors.Black;
            }

            TextChanged?.Invoke(this, TextChangedEventArgs.Empty);
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
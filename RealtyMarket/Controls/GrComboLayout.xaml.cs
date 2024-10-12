using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using System.Collections.ObjectModel;

namespace RealtyMarket.Controls
{
    public partial class GrComboLayout : ContentView
    {
        public static readonly BindableProperty ItemsProperty = BindableProperty.Create(
           nameof(Items),
           typeof(ObservableCollection<string>),
           typeof(GrComboLayout),
           new ObservableCollection<string>(),
           propertyChanged: OnItemsChanged);

        public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(
            nameof(FontFamily), typeof(string), typeof(GrComboLayout), "NexaTrialBold", BindingMode.OneWay, propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (GrComboLayout)bindable;
                control.TitleLabel.FontFamily = newValue as string;
            });

        public static readonly BindableProperty SelectColorProperty = BindableProperty.Create(
            nameof(SelectColor), typeof(Color), typeof(GrComboLayout), Colors.DarkViolet, BindingMode.OneWay);

        public ObservableCollection<string> Items
        {
            get => (ObservableCollection<string>)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        public static readonly BindableProperty TitleProperty = BindableProperty.Create(
            nameof(Title), typeof(string), typeof(GrComboLayout), "Title", propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (GrComboLayout)bindable;
                control.TitleLabel.Text = newValue as string;
            });

        public string FontFamily
        {
            get => GetValue(FontFamilyProperty) as string;
            set => SetValue(FontFamilyProperty, value);
        }

        public Color SelectColor
        {
            get => GetValue(SelectColorProperty) as Color;
            set => SetValue(SelectColorProperty, value);
        }

        public string Title
        {
            get => GetValue(TitleProperty) as string;
            set => SetValue(TitleProperty, value);
        }

        private readonly FlexLayout _flexLayout;

        public GrComboLayout()
        {
            InitializeComponent();

            _flexLayout = new FlexLayout()
            {
                Margin = new Thickness(0, 0, 0, 0),
                Wrap = Microsoft.Maui.Layouts.FlexWrap.Wrap,
                AlignItems = Microsoft.Maui.Layouts.FlexAlignItems.Start
            };
            MainLayout.Children.Add(_flexLayout);
            MainLayout.SetRow(_flexLayout, 1);
        }

        private static void OnItemsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (GrComboLayout)bindable;

            if (oldValue is ObservableCollection<string> oldCollection)
                oldCollection.CollectionChanged -= control.OnCollectionChanged;

            if (newValue is ObservableCollection<string> newCollection)
            {
                newCollection.CollectionChanged += control.OnCollectionChanged;
                control.UpdateItems();
            }
        }

        private void OnCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateItems();
        }

        public string _selectedText = string.Empty;

        public string SelectedText
        {
            get => _selectedText;
        }

        public int _selectedIndex = -1;

        public int SelectedIndex
        {
            get => _selectedIndex;
        }

        private void UpdateItems()
        {
            _flexLayout.Children.Clear();
            for (int i = 0; i < Items.Count; i++)
            {
                var item = Items[i];

                var border = new Border
                {
                    Stroke = new SolidColorBrush(Colors.Black),
                    StrokeThickness = 2,
                    HeightRequest = 40,
                    MinimumWidthRequest = 40,
                    Padding = new Thickness(10,0,10,0),
                    Margin = new Thickness(5, 5, 0, 0),
                    StrokeShape = new RoundRectangle() { CornerRadius = 20 },
                };

                var label = new Label
                {
                    Text = item,
                    TextColor = Colors.Black,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    FontSize = 14
                };

                label.SetBinding(Label.FontFamilyProperty, new Binding(nameof(FontFamilyProperty), BindingMode.OneWay, source: this));

                border.Content = label;

                border.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(() => OnFrameTapped(border, i))
                });

                _flexLayout.Children.Add(border);
            }
        }

        private Border _selectedFrame;

        private void OnFrameTapped(Border frame, int index)
        {
            if (_selectedFrame != null)
            {
                _selectedFrame.BackgroundColor = Colors.White;
                if (_selectedFrame.Content is Label labelOld)
                {
                    labelOld.TextColor = Colors.Black;
                }
            }

            if (_selectedFrame != frame)
            {
                _selectedFrame = frame;
                _selectedFrame.BackgroundColor = SelectColor;

                if (_selectedFrame.Content is Label labelNew)
                {
                    labelNew.TextColor = Colors.White;
                    _selectedText = labelNew.Text;
                }
                _selectedIndex = index;
            }
            else
            {
                _selectedFrame = null;
                _selectedText = string.Empty;
                _selectedIndex = -1;
            }
        }
    }
}
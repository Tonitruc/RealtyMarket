using System.Collections.ObjectModel;

namespace RealtyMarket.Controls
{
    public partial class RegistrationAdvantagesLabel : ContentView
    {
        public static readonly BindableProperty TitlePropery = BindableProperty.Create(
            nameof(Title), typeof(string), typeof(RegistrationAdvantagesLabel), propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (RegistrationAdvantagesLabel)bindable;
                control.TitleLabel.Text = newValue as string;
            });


        public string Title
        {
            get => GetValue(TitlePropery) as string;
            set => SetValue(TitlePropery, value);
        }

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
            nameof(ItemsSource), typeof(ObservableCollection<RegistrationAdvantage>), typeof(RegistrationAdvantagesLabel), new ObservableCollection<RegistrationAdvantage>(), BindingMode.TwoWay,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (RegistrationAdvantagesLabel)bindable;
                var test = newValue as ObservableCollection<RegistrationAdvantage>;
                control.RegistrationAdvantagesCollectionView.ItemsSource = newValue as ObservableCollection<RegistrationAdvantage>;
            });

        public ObservableCollection<RegistrationAdvantage> ItemsSource
        {
            get => (ObservableCollection<RegistrationAdvantage>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public RegistrationAdvantagesLabel()
        {
            InitializeComponent();

            ItemsSource = [
                new() { Text = "Возможность публикации объявлений."},
                new() { Text = "Личный кабинет."},
                new() { Text = "Избранное и история."}
            ];

            BindingContext = this;
        }
    }

    public class RegistrationAdvantage
    {
        public string Text { get; set; }
        public double FontSize { get; set; } = 13;
    }
}
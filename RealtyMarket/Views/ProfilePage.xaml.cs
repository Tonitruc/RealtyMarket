using RealtyMarket.ViewModels;

namespace RealtyMarket.Views
{
    public partial class ProfilePage : ContentPage
    {
        private ProfileViewModel _profileViewModel;

        public ProfilePage(ProfileViewModel profileViewModel)
        {
            InitializeComponent();

            BindingContext = _profileViewModel = profileViewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            _profileViewModel.IsLoading = true;

            await _profileViewModel.InitializeUserStateAsync();

            _profileViewModel.IsLoading = false;
        }
    }
}
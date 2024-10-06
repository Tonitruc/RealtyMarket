using RealtyMarket.Service;

namespace RealtyMarket
{
    public partial class AppShell : Shell
    {
        private readonly SecureStorageUserRepository _secureStorageUserRepository;

        private readonly ConnectivityService _connectivityService;

        public AppShell(SecureStorageUserRepository secureStorageUserRepository,
            ConnectivityService connectivityService)
        {
            InitializeComponent();

            _secureStorageUserRepository = secureStorageUserRepository;
            _connectivityService = connectivityService;

            Task.Run(CheckUserAuthenticationAsync);

            Navigating += CheckConnetionNavigating;
        }

        private async void CheckConnetionNavigating(object sender, ShellNavigatingEventArgs e)
        {
            if (!_connectivityService.IsConnectedToInternet())
            {
                if (!e.Target.Location.OriginalString.StartsWith("//NoInternetPage"))
                {
                    e.Cancel();
                    await DisplayAlert("", $"{e.Target.Location.OriginalString}", "Ok");
                    await GoToAsync($"//NoInternetPage?ReturnPage={e.Target.Location.OriginalString}");
                }
            }
        }

        private async Task CheckUserAuthenticationAsync()
        {
            bool isLoggedIn = _secureStorageUserRepository.UserExists();

            if (isLoggedIn)
            {
                await Dispatcher.DispatchAsync(() =>
                {
                    GoToAsync("//MainTabs");
                });
            }
            else
            {
                await Dispatcher.DispatchAsync(() =>
                {
                    GoToAsync("//LoginRegisterPage");
                });
            }
        }
    }
}

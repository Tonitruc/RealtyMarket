using RealtyMarket.Service;

namespace RealtyMarket
{
    public partial class AppShell : Shell
    {
        private readonly SecureStorageUserRepository _secureStorageUserRepository;

        private readonly ConnectivityService _connectivityService;

        private readonly List<string> NeedRegisterPages;

        public AppShell(SecureStorageUserRepository secureStorageUserRepository,
            ConnectivityService connectivityService)
        {
            InitializeComponent();

            _secureStorageUserRepository = secureStorageUserRepository;
            _connectivityService = connectivityService;

            Navigating += CheckConnetionNavigating;

            NeedRegisterPages = [
                "//FavoritesPage",
                "//AddAdPage",
                "//MyAdsPage",
                "//MainTabs/AddAdPage",
                "//MainTabs/FavoritesPage"
                ];
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            bool isLoggedIn = await _secureStorageUserRepository.UserExists();
            if(isLoggedIn)
            {
                await GoToAsync("//MainTabs");
            }
            else
            {
                await GoToAsync("//LoginPage");
            }
        }

        private async void CheckConnetionNavigating(object sender, ShellNavigatingEventArgs e)
        {
            if (!_connectivityService.IsConnectedToInternet())
            {
                if (!e.Target.Location.OriginalString.StartsWith("//NoInternetPage"))
                {
                    e.Cancel();
                    await GoToAsync($"//NoInternetPage?ReturnPage={e.Target.Location.OriginalString}");
                }
            }
            else
            {
                CheckUserAuthenticationAsync(sender, e);
            }
        }

        private async void CheckUserAuthenticationAsync(object sender, ShellNavigatingEventArgs e)
        {
            var newPage = e.Target.Location.OriginalString;
            if (!_connectivityService.IsConnectedToInternet())
            {
                await GoToAsync($"//NoInternetPage?ReturnPage={newPage}");
            }
            else
            {
                var user = await _secureStorageUserRepository.ReadUser();
                var userStatus = await _secureStorageUserRepository.GetUserState();

                if (userStatus == "Guest" && NeedRegisterPages.Contains(newPage))
                {
                    e.Cancel();
                    await GoToAsync("//LoginPage");
                }
            }
        }
    }
}

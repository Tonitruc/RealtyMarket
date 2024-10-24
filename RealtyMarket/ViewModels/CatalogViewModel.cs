using CommunityToolkit.Mvvm.ComponentModel;
using RealtyMarket.Models;
using RealtyMarket.Repository;
using RealtyMarket.Service;
using System.Collections.ObjectModel;
using System.Reactive.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

namespace RealtyMarket.ViewModels
{
    public partial class AdvertisementItem : ObservableObject
    {
        [ObservableProperty]
        private Advertisement _advertisement;

        [ObservableProperty]
        private bool _isFavorite = false;

        [ObservableProperty]
        private bool _isRegistered = false;
    }


    public partial class CatalogViewModel : ObservableObject
    {
        private readonly AdvertisementRepository _advertisementRepository;

        private readonly RegisteredUserRepository _registeredUserRepository;

        private readonly SecureStorageUserRepository _secureStorageUserRepository;

        [ObservableProperty]
        public ObservableCollection<AdvertisementItem> _advertisements;

        private bool _isLoading = false;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref  _isLoading, value);
        }

        public RegisteredUser User { get; set; }


        public CatalogViewModel(AdvertisementRepository advertisementRepository,
            RegisteredUserRepository registeredUserRepository, SecureStorageUserRepository secureStorageUserRepository)
        { 
            _advertisementRepository = advertisementRepository;
            _registeredUserRepository = registeredUserRepository;
            _secureStorageUserRepository = secureStorageUserRepository;
            Advertisements = [];
        }

        public async Task GetAdvertisements()
        {
            var advertisementsList = await _advertisementRepository.GetAll();
            Advertisements.Clear();
            foreach(var ad in  advertisementsList)
            {
                Advertisements.Add(new AdvertisementItem() { Advertisement = ad, IsFavorite = IsFavorite(ad), 
                    IsRegistered = User != null});
            }
        }

        public async Task GetUser()
        {
            var userInfo = await _secureStorageUserRepository.ReadUser();

            if(userInfo.userInfo.IsAnonymous)
            {
                User = null;
                return;
            }

            var email = userInfo.userInfo.Email;

            User = await _registeredUserRepository.GetByEmail(email);
        }

        public async Task AddFavorite(AdvertisementItem ad)
        {
            await _registeredUserRepository.AddFavorite(User.Email, ad.Advertisement.Id);
            ad.IsFavorite = true;
        }

        public async Task DeleteFavorite(AdvertisementItem ad)
        {
            await _registeredUserRepository.DeleteFavorite(User.Email, ad.Advertisement.Id);
            ad.IsFavorite = false;
        }

        public bool IsFavorite(Advertisement ad)
        {
            if(User == null)
            {
                return false;
            }

            return User.Favorites.Contains(ad.Id);
        }
    }
}

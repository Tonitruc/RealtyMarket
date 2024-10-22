using CommunityToolkit.Mvvm.ComponentModel;
using RealtyMarket.Models;
using RealtyMarket.Repository;
using RealtyMarket.Service;
using System.Collections.ObjectModel;
using System.Reactive.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

namespace RealtyMarket.ViewModels
{
    public class AdvertisementItem : ObservableObject
    {
        public Advertisement Advertisement { get; set; }

        private bool _isFavorite = false;

        public bool IsFavorite
        {
            get => _isFavorite;
            set => SetProperty(ref _isFavorite, value);
        }
    }


    public class CatalogViewModel : ObservableObject
    {
        private readonly AdvertisementRepository _advertisementRepository;

        private readonly RegisteredUserRepository _registeredUserRepository;

        private readonly SecureStorageUserRepository _secureStorageUserRepository;

        public ObservableCollection<AdvertisementItem> Advertisements { get; }

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
                Advertisements.Add(new AdvertisementItem() { Advertisement = ad, IsFavorite = IsFavorite(ad) });
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

        public async Task AddFavorite(Advertisement ad)
        {
            await _registeredUserRepository.AddFavorite(User.Email, ad.Id);
        }

        public async Task DeleteFavorite(Advertisement ad)
        {
            await _registeredUserRepository.DeleteFavorite(User.Email, ad.Id);
        }

        public bool IsFavorite(Advertisement ad)
        {
            return User.Favorites.Contains(ad.Id);
        }
    }
}

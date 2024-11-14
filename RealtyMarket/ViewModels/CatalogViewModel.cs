using CommunityToolkit.Mvvm.ComponentModel;
using RealtyMarket.Models;
using RealtyMarket.Models.RealtyEntity.Enums;
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
        private ObservableCollection<AdvertisementItem> _advertisements;

        [ObservableProperty]
        private ObservableCollection<AdvertisementItem> _filteredAdvertisement;

        private bool _isLoading = false;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref  _isLoading, value);
        }

        public RegisteredUser User { get; set; }

        public string SearchAddress { get; set; } = string.Empty;

        public CatalogViewModel(AdvertisementRepository advertisementRepository,
            RegisteredUserRepository registeredUserRepository, SecureStorageUserRepository secureStorageUserRepository)
        { 
            _advertisementRepository = advertisementRepository;
            _registeredUserRepository = registeredUserRepository;
            _secureStorageUserRepository = secureStorageUserRepository;
            Advertisements = [];
        }

        public async Task GetAdvertisements(RealtyFilter filter = null)
        {
            if(filter == null)
            {
                filter = new();
            }

            var advertisementsList = await _advertisementRepository.GetAll();
            Advertisements.Clear();
            foreach(var ad in  advertisementsList)
            {
                Advertisements.Add(new AdvertisementItem() { Advertisement = ad, IsFavorite = IsFavorite(ad), 
                    IsRegistered = User != null});
            }

            FilterAdvertisement(filter);
        }

        public async Task SearchAddressCommand(RealtyFilter filter = null)
        {
            await GetAdvertisements(filter);

            if (filter == null)
            {
                filter = new();
            }

            if (string.IsNullOrEmpty(SearchAddress))
            {
                return;
            }

            List<AdvertisementItem> list = FilteredAdvertisement.Where(ad => 
            ad.Advertisement.Realty.Location.Address.Contains(SearchAddress)).ToList();

            FilteredAdvertisement.Clear();

            foreach(var item in list)
            {
                FilteredAdvertisement.Add(item);
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
            User.Favorites.Add(ad.Advertisement.Id);
            ad.IsFavorite = true;
        }

        public async Task DeleteFavorite(AdvertisementItem ad)
        {
            await _registeredUserRepository.DeleteFavorite(User.Email, ad.Advertisement.Id);
            User.Favorites.Remove(ad.Advertisement.Id);
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

        public void FilterAdvertisement(RealtyFilter filter)
        {
            List<AdvertisementItem> sortedAds = new(Advertisements);
            
            if(filter.RealtyCategory != "Не выбрано")
            {
                sortedAds = sortedAds.Where(ad => ad.Advertisement.RealtyCategory == filter.RealtyCategory).ToList();
            }
            if (!string.IsNullOrEmpty(filter.AdType))
            {
                sortedAds = sortedAds.Where(ad => ad.Advertisement.Type == filter.AdType).ToList();
            }
            if(!string.IsNullOrEmpty(filter.Region))
            {
                sortedAds = sortedAds.Where(ad => ad.Advertisement.Realty.Location.Address.Contains(filter.Region)).ToList();
            }

            sortedAds = filter.Sort switch
            {
                RealtySort.New => sortedAds.OrderByDescending(ad => ad.Advertisement.PublishDate).ToList(),
                RealtySort.CostDescending => sortedAds.OrderBy(ad => ad.Advertisement.Cost["USD"]).ToList(),
                RealtySort.CostAscending => sortedAds.OrderByDescending(ad => ad.Advertisement.Cost["USD"]).ToList()
            };

            FilteredAdvertisement = new(sortedAds);
        }
    }
}

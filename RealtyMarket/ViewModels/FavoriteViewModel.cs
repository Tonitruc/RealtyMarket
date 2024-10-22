using CommunityToolkit.Mvvm.ComponentModel;
using RealtyMarket.Models;
using RealtyMarket.Repository;
using RealtyMarket.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtyMarket.ViewModels
{
    public class FavoriteViewModel : ObservableObject
    {
        private readonly AdvertisementRepository _advertisementRepository;

        private readonly RegisteredUserRepository _registeredUserRepository;

        private readonly SecureStorageUserRepository _secureStorageUserRepository;

        public ObservableCollection<Advertisement> Advertisements { get; }

        private bool _isLoading = false;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public RegisteredUser User { get; set; }


        public FavoriteViewModel(AdvertisementRepository advertisementRepository,
            RegisteredUserRepository registeredUserRepository, SecureStorageUserRepository secureStorageUserRepository)
        {
            _secureStorageUserRepository = secureStorageUserRepository;
            _registeredUserRepository = registeredUserRepository;
            _advertisementRepository = advertisementRepository;

            Advertisements = [];
        }

        public async Task GetAdvertisements()
        {
            var advertisementsList = await _advertisementRepository.GetFavorites(User);
            Advertisements.Clear();
            foreach (var ad in advertisementsList)
            {
                Advertisements.Add(ad);
            }
        }
        public async Task GetUser()
        {
            var userInfo = await _secureStorageUserRepository.ReadUser();

            if (userInfo.userInfo.IsAnonymous)
            {
                User = null;
                return;
            }

            var email = userInfo.userInfo.Email;

            User = await _registeredUserRepository.GetByEmail(email);
        }

        public async Task DeleteFavorite(Advertisement ad)
        {
            await _registeredUserRepository.DeleteFavorite(User.Email, ad.Id);
        }
    }
}

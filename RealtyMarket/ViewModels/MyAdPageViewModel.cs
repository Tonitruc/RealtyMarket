using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using MvvmHelpers.Commands;
using RealtyMarket.Controls;
using RealtyMarket.Models;
using RealtyMarket.Repository;
using RealtyMarket.Service;

namespace RealtyMarket.ViewModels
{
    public class MyAdPageViewModel : ObservableObject
    {
        public ICommand ReturnCommand { get; set; }


        private readonly AdvertisementRepository _advertisementRepository;

        private readonly SecureStorageUserRepository _secureStorageUserRepository;

        private string UserEmail;

        public ObservableCollection<Advertisement> ActiveAds { get; }

        public ObservableCollection<Advertisement> ClosedAds { get; }

        private int _amountActiveAds = 0;
        public int AmountActiveAds
        {
            get => _amountActiveAds;
            set
            {
                SetProperty(ref _amountActiveAds, value);
                OnPropertyChanged(nameof(IsZeroActiveAds));
            }
        }

        private int _amountClosedAds = 0;
        public int AmountClosedAds
        {
            get => _amountClosedAds;
            set
            {
                SetProperty(ref _amountClosedAds, value);
                OnPropertyChanged(nameof(IsZeroClosedAds));
            }
        }

        public bool IsZeroActiveAds => _amountActiveAds == 0;
        public bool IsZeroClosedAds => _amountClosedAds == 0;

        private bool _isLoading = false;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public MyAdPageViewModel(AdvertisementRepository advertisementRepository,
            SecureStorageUserRepository secureStorageUserRepository)
        {
            ReturnCommand = new AsyncCommand(ReturnToProfilePage);
            ActiveAds = []; ClosedAds = [];

            _advertisementRepository = advertisementRepository;
            _secureStorageUserRepository = secureStorageUserRepository;
        }

        public async Task GetUserEmail()
        {
            var userInfo = await _secureStorageUserRepository.ReadUser();
            string email = userInfo.userInfo.Email;

            UserEmail = email;
        }

        public async Task GetMyAds()
        {
            ActiveAds.Clear();
            AmountActiveAds = 0;
            var activeAdvertisements = await _advertisementRepository.GetByEmail(UserEmail);
            foreach(var ad in activeAdvertisements)
            {
                ActiveAds.Add(ad);
                AmountActiveAds++;
            }

            OnPropertyChanged(nameof(AmountActiveAds));

            ClosedAds.Clear();
            AmountClosedAds = 0;
            var closedAdvertisements = await _advertisementRepository.GetByEmail(UserEmail, false);
            foreach(var ad in closedAdvertisements)
            {
                ClosedAds.Add(ad);
                AmountClosedAds++;
            }

            OnPropertyChanged(nameof(AmountClosedAds));
        }

        public async Task ReturnToProfilePage()
        {
            await Shell.Current.GoToAsync("//ProfilePage");
        }

        public async Task CloseAdCommand(Advertisement ad)
        {
            await _advertisementRepository.AdStatusChange(ad);
            await GetMyAds();
        }

        public async Task OpenAdCommand(Advertisement ad)
        {
            await _advertisementRepository.AdStatusChange(ad, false);
            await GetMyAds();
        }

        public async Task DeleteAdCommand(Advertisement ad)
        {
            await _advertisementRepository.DeleteAd(ad);
            await GetMyAds();
        }
    }
}

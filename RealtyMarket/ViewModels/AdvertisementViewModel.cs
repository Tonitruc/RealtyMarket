using CommunityToolkit.Mvvm.ComponentModel;
using RealtyMarket.Models;
using RealtyMarket.Repository;
using RealtyMarket.Service;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace RealtyMarket.ViewModels
{
    public partial class AdvertisementViewModel : ObservableObject
    {
        [ObservableProperty]
        private Advertisement _advertisement;

        private readonly RegisteredUserRepository _registeredUserRepository;

        private readonly SecureStorageUserRepository _secureStorageUserRepository;

        private readonly ImageBBRepository _imageBBRepositroy;

        public RegisteredUser User { get; set; }

        public ICommand ReturnCommand { get; }

        public ObservableCollection<ImageSource> Photos { get; }

        [ObservableProperty]
        public bool _isLoading = true;

        public AdvertisementViewModel(RegisteredUserRepository registeredUserRepository,
            SecureStorageUserRepository secureStorageUserRepository, ImageBBRepository imageBBRepository)
        {
            _registeredUserRepository = registeredUserRepository;
            _secureStorageUserRepository = secureStorageUserRepository;
            _imageBBRepositroy = imageBBRepository;

            ReturnCommand = new AsyncCommand(Return);

            Photos = [];
        }

        public async Task Return()
        {
            await Shell.Current.GoToAsync("//CatalogPage");
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

        public void LoadPhoto()
        {
            Photos.Clear();

            if(Advertisement.ImageUrls.Count == 0)
            {
                Photos.Add(ImageSource.FromFile("ad_logo.png"));
            }

            foreach(var photoUrl in Advertisement.ImageUrls)
            {
                Photos.Add(_imageBBRepositroy.Get(photoUrl));
            }
        }
    }
}

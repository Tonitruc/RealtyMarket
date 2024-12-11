using CommunityToolkit.Mvvm.ComponentModel;
using RealtyMarket.Models;
using RealtyMarket.Repository;
using RealtyMarket.Service;
using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Prism.Navigation.Xaml;
using Firebase.Database;

namespace RealtyMarket.ViewModels
{
    public partial class AdvertisementImageItem : ObservableObject
    {
        [ObservableProperty]
        private ImageSource _imageSource;
    }

    public partial class AdvertisementViewModel : ObservableObject
    {
        [ObservableProperty]
        private Advertisement _advertisement;

        private readonly ImageBBRepository _imageBBRepositroy;
        private readonly RegisteredUserRepository _registeredUserRepository;

        public RegisteredUser User { get; set; }

        public ObservableCollection<AdvertisementImageItem> Photos { get; }

        [ObservableProperty]
        public bool _isLoading = true;

        [ObservableProperty]
        public bool _isFavorite = false;

        [ObservableProperty]
        public bool _isSelfAd = false;


        public AdvertisementViewModel()
        {
            using HttpClient httpClient = new();
            _imageBBRepositroy = new ImageBBRepository(httpClient);

            Photos = new ObservableCollection<AdvertisementImageItem>();

            FirebaseClient firebaseClient = new FirebaseClient("https://realtymarket-e4db0-default-rtdb.firebaseio.com/");
            _registeredUserRepository = new RegisteredUserRepository(firebaseClient);
        }

        public void LoadPhoto()
        {
            Photos.Clear();

            if (Advertisement.ImageUrls == null || Advertisement.ImageUrls.Count == 0)
            {
                Photos.Add(new() { ImageSource = ImageSource.FromFile("ad_logo.png") });
            }
            else
            {
                foreach (var photoUrl in Advertisement.ImageUrls)
                {
                    var image = _imageBBRepositroy.Get(photoUrl);
                    if (image != null)
                    {
                        Photos.Add(new() { ImageSource = image });
                    }
                }
            }
        }

        public void IsFavoriteCheck()
        {
            IsFavorite = User.Favorites.Contains(Advertisement.Id);
            IsSelfAd = true; //Advertisement.UserEmail != User.Email;
        }

        public async Task AddFavorite(Advertisement ad)
        {
            await _registeredUserRepository.AddFavorite(User.Email, ad.Id);
            User.Favorites.Add(ad.Id);
            IsFavorite = true;
        }

        public async Task DeleteFavorite(Advertisement ad)
        {
            await _registeredUserRepository.DeleteFavorite(User.Email, ad.Id);
            User.Favorites.Remove(ad.Id);
            IsFavorite = false;
        }
    }
}

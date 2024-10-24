using CommunityToolkit.Mvvm.ComponentModel;
using RealtyMarket.Models;
using RealtyMarket.Repository;
using RealtyMarket.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtyMarket.ViewModels
{
    public partial class AdvertisementViewModel : ObservableObject
    {
        [ObservableProperty]
        private Advertisement _advertisement;

        private readonly RegisteredUserRepository _registeredUserRepository;

        private readonly SecureStorageUserRepository _secureStorageUserRepository;

        public RegisteredUser User { get; set; }


        public AdvertisementViewModel(RegisteredUserRepository registeredUserRepository,
            SecureStorageUserRepository secureStorageUserRepository)
        {
            _registeredUserRepository = registeredUserRepository;
            _secureStorageUserRepository = secureStorageUserRepository;
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
    }
}

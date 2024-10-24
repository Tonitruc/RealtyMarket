using RealtyMarket.ViewModels;
using System.Windows.Input;
using MvvmHelpers.Commands;
using RealtyMarket.Service;
using RealtyMarket.Repository;
using RealtyMarket.Models;
using MvvmHelpers;

namespace RealtyMarket.ViewModels
{
    public class UserSettingsViewModel : ObservableObject
    {
        private readonly SecureStorageUserRepository _secureStorageUserRepository;

        private readonly RegisteredUserRepository _registeredUserRepository;

        private readonly ImageBBRepository _imageBBRepository;

        public ICommand CancelChangesCommand { get; }

        public ICommand SaveChangesCommand { get; }

        public ICommand PickAvatarPhotoCommand { get; }

        private RegisteredUser _user;

        public RegisteredUser User
        {
            get => _user;
            private set => SetProperty(ref _user, value);
        }

        private bool _isLoading = false;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        private ImageSource _avatarPhoto;

        public ImageSource AvatarPhoto
        {
            get => _avatarPhoto;
            set => SetProperty(ref _avatarPhoto, value);
        }


        public UserSettingsViewModel(SecureStorageUserRepository secureStorageUserRepository,
            RegisteredUserRepository registeredUserRepository, ImageBBRepository imageBBRepository)
        {
            CancelChangesCommand = new AsyncCommand(CancelChanges);
            PickAvatarPhotoCommand = new AsyncCommand(PickAvatarPhoto);
            SaveChangesCommand = new AsyncCommand(SaveChanges);

            _secureStorageUserRepository = secureStorageUserRepository;
            _registeredUserRepository = registeredUserRepository;
            _imageBBRepository = imageBBRepository;
        }

        public async Task GetUserInfo()
        {
            var userInfo = await _secureStorageUserRepository.ReadUser();
            string email = userInfo.userInfo.Email;

            User = await _registeredUserRepository.GetByEmail(email);
            if(!string.IsNullOrEmpty(User.UserImageUrl))
            {
                AvatarPhoto = _imageBBRepository.Get(User.UserImageUrl);
            }
        }

        public async Task CancelChanges()
        {
            await Shell.Current.GoToAsync("//ProfilePage");
        }

        public async Task PickAvatarPhoto()
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Выберите изображение.",
                    FileTypes = FilePickerFileType.Images,
                });

                if (result != null)
                {
                    using (var stream = await result.OpenReadAsync())
                    {
                        var memoryStream = new MemoryStream();
                        await stream.CopyToAsync(memoryStream);
                        var imageData = memoryStream.ToArray();

                        var imageSource = ImageSource.FromStream(() => new MemoryStream(imageData));

                        AvatarPhoto = imageSource;
                    }
                }
            }
            catch (Exception) { }
        }

        public async Task SaveChanges()
        {
            string imageUrl = string.Empty;
            if(string.IsNullOrEmpty(User.UserImageUrl) && AvatarPhoto != null)
            {
                imageUrl = await _imageBBRepository.Add(AvatarPhoto);
            }
            User.UserImageUrl = imageUrl;

            await _registeredUserRepository.UpdateUser(User, User.Id);
            await Shell.Current.GoToAsync("//ProfilePage");
        }
    }
}

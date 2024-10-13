using CommunityToolkit.Mvvm.ComponentModel;
using RealtyMarket.Models;
using RealtyMarket.Models.RealtyEntity;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MvvmHelpers.Commands;
using RealtyMarket.Service;
using RealtyMarket.Repository;


namespace RealtyMarket.ViewModels
{
    public class PhotoItem
    {
        public bool IsFirst { get; set; } = false;
        public ImageSource ImageSource { get; set; } = null;
        public bool IsPhoto => ImageSource != null;
        public bool IsAddButton => ImageSource == null;  
    }

    public class AddAdViewModel : ObservableObject
    {

        public bool _isVisible;
        public bool IsVisible
        {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);
        }

        private bool _isLoading = false;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        private readonly SecureStorageUserRepository _secureStorageUserRepository;

        private readonly RegisteredUserRepository _registeredUserRepository;

        public RegisteredUser User { get; set; }

        public Advertisement Advertisement { get; set; }

        #region AdvertismentParam

        private int _amountPhotos = 0;
        public int AmountPhotos
        {
            get => _amountPhotos;
            set => SetProperty(ref _amountPhotos, value);
        }

        public ObservableCollection<PhotoItem> Photos { get; private set; }

        public MvvmHelpers.Commands.Command AddPhotoCommand { get; }

        public MvvmHelpers.Commands.Command<PhotoItem> RemovePhotoCommand { get; }

        public ObservableCollection<string> RealtyCategories { get; } = [
                "Не выбрано",
                "Квартира",
                "Дом"
                ];

        public ObservableCollection<string> Currencys { get; } = [
            "$", "€", "BYN", "Z$"
            ];

        #endregion

        #region ResidentialRealtyParam

        public ObservableCollection<string> AmountRooms { get; } = [
            "1", "2", "3", "4", "5+" ];

        public ObservableCollection<string> AmountFloors { get; } = [
            "1", "2", "3+" ];

        public ObservableCollection<string> CeilingHeight { get; } = [
            "от 2.5 м", "от 2.7 м", "от 3 м", "от 3.5 м", "от 4 м" ];

        public ObservableCollection<string> NewnessTypes { get; } = [
            "Новое", "Вторичное"
        ];

        #endregion

        #region FlatParam

        public ObservableCollection<string> FlatToilet { get; } = [
            "Раздельный", "Совмещенный", "Нету"
            ];

        public ObservableCollection<string> BalconyTypes { get; } = [
            "Есть", "Нету", "Лоджия"
        ];

        public ObservableCollection<string> RepairTypes { get; } = [
            "Евро", "Косметический", "Дизайнерский", "Строительная отделка", 
            "Без отделки", "Аварийное состояние"
        ];

        public ObservableCollection<string> FlatConvenitnces { get; } = [
            "Лифт",
            "Пандус",
            "Мусоропровод",
            "Закрытый двор",
            "Парковка",
            "Домофон",
            "Подвал",
            "Видеонаблюдение",
            "Двор для выгула мопсов"
        ];

        #endregion

        #region HouseParam

        public ObservableCollection<string> HouseToilet { get; } = [
            "В доме", "На улице", "Выгребная яма", "Биологическое разложение", "C/y на улице"
            ];

        public ObservableCollection<string> GasSystems { get; } = [
            "В доме", "На улице", "Выгребная яма", "Биологическое разложение", "C/y на улице"
        ];

        #endregion

        public ICommand ReturnCommand { get; set; }

        public AddAdViewModel(SecureStorageUserRepository secureStorageUserRepository,
            RegisteredUserRepository registeredUserRepotisotry)
        {
            Photos = new ObservableCollection<PhotoItem>();

            Photos.Add(new PhotoItem() { IsFirst = true });

            for (int i = 1; i < 10; i++)
            {
                Photos.Add(new PhotoItem());  
            }

            AddPhotoCommand = new MvvmHelpers.Commands.Command(async () => await AddPhotoAsync());
            RemovePhotoCommand = new MvvmHelpers.Commands.Command<PhotoItem>(RemovePhoto);

            Advertisement = new Advertisement() { Realty = new ResidentialRealty()};

            ReturnCommand = new AsyncCommand(ReturnToMainTabs);

            _secureStorageUserRepository = secureStorageUserRepository;
            _registeredUserRepository = registeredUserRepotisotry;
        }

        public async Task GetUserInfo()
        {
            var userInfo = _secureStorageUserRepository.ReadUser();
            User = await _registeredUserRepository.GetByEmail(userInfo.userInfo.Email);
        }

        public async Task ReturnToMainTabs()
        {
            await Shell.Current.GoToAsync("//CatalogPage");
        }


        #region Work with photo

        private async Task AddPhotoAsync()
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = FilePickerFileType.Images,
                    PickerTitle = "Выберите изображение"
                });

                if (result != null)
                {
                    var stream = await result.OpenReadAsync();
                    var imageSource = ImageSource.FromStream(() => stream);

                    int addButtonIndex = Photos.IndexOf(Photos.First(x => x.IsAddButton));
                    if (addButtonIndex != -1)
                    {
                        Photos[addButtonIndex].ImageSource = imageSource;

                        if (addButtonIndex + 1 < Photos.Count)
                        {
                            Photos[addButtonIndex + 1] = new PhotoItem();
                            _amountPhotos++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", ex.Message, "ОК");
            }
        }

        private void RemovePhoto(PhotoItem photo)
        {
            if (photo.IsPhoto)
            {
                int photoIndex = Photos.IndexOf(photo);
                Photos[photoIndex] = new PhotoItem();

                int addButtonIndex = Photos.IndexOf(Photos.First(x => x.IsAddButton));
                if (addButtonIndex > photoIndex)
                {
                    Photos.Move(addButtonIndex, photoIndex); 
                }
            }
        }

        #endregion
    }
}

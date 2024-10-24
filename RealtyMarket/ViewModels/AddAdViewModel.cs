using CommunityToolkit.Mvvm.ComponentModel;
using RealtyMarket.Models;
using RealtyMarket.Models.RealtyEntity;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MvvmHelpers.Commands;
using RealtyMarket.Service;
using RealtyMarket.Repository;
using Microsoft.Maui.Controls;


namespace RealtyMarket.ViewModels
{
    public partial class PhotoItem : ObservableObject
    {
        [ObservableProperty]
        public bool _isButton;

        private ImageSource _imageSource;

        public ImageSource ImageSource
        {
            get => _imageSource;
            set
            {
                SetProperty(ref _imageSource, value);
                OnPropertyChanged(nameof(IsEmpty));
            }
        }

        public bool IsEmpty => ImageSource != null;
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

        public bool NeedClear = false;

        private readonly SecureStorageUserRepository _secureStorageUserRepository;

        private readonly RegisteredUserRepository _registeredUserRepository;

        public RegisteredUser User { get; set; }

        public Advertisement Advertisement { get; set; }

        #region AdvertismentParam

        public ObservableCollection<string> AdvertisementTypes { get; } = [
            "Продажа",
            "Аренда"
            ];


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

        public ObservableCollection<string> HouseTypes { get; } = [
            "Дом", "Коттедж", "Дача", "Таунхаус", "Другое"
            ];

        public ObservableCollection<string> WaterTypes { get; } = [
            "Сезонная", "Скважина", "Колодец", "Центральная", "Отсутствует"
            ];

        public ObservableCollection<string> SewerageSystemTypes { get; } = [
            "В доме", "На улице", "Выгребная яма", "Биологическое разложение", "C/y на улице"
            ];

        public ObservableCollection<string> GasSystems { get; } = [
            "В доме", "Подведен к дому", "Нету"
        ];

        public ObservableCollection<string> TerritoryConveniences { get; } = [
            "Рядом водоем", "Рядом лес", "Баня", "Беседка", "Мангал", "Бассейн", 
            "Другие постройки"
            ];

        #endregion

        public ICommand ReturnCommand { get; set; }

        private readonly AdvertisementRepository _advertisementRepository;

        public AddAdViewModel(SecureStorageUserRepository secureStorageUserRepository,
            RegisteredUserRepository registeredUserRepotisotry, 
            AdvertisementRepository advertisementRepository,
            ImageBBRepository imageBBRepository)
        {
            Photos = [new() { IsButton = true }];
            for (int i = 0; i < 9; i++)
            {
                Photos.Add(new());
            }

            AddPhotoCommand = new AsyncCommand(PickImageAsync);
            RemovePhotoCommand = new MvvmHelpers.Commands.Command<PhotoItem>(RemovePhotoAsync);

            Advertisement = new Advertisement() { Realty = new ResidentialRealty()};

            ReturnCommand = new AsyncCommand(ReturnToMainTabs);

            _secureStorageUserRepository = secureStorageUserRepository;
            _registeredUserRepository = registeredUserRepotisotry;
            _advertisementRepository = advertisementRepository;
            _imageBBRepository = imageBBRepository;
        }

        public async Task GetUserInfo()
        {
            var userInfo = await _secureStorageUserRepository.ReadUser();
            User = await _registeredUserRepository.GetByEmail(userInfo.userInfo.Email);
        }

        public async Task ReturnToMainTabs()
        {
            await Shell.Current.GoToAsync("//CatalogPage");
        }


        #region Work with photo

        private readonly ImageBBRepository _imageBBRepository;

        public ICommand AddPhotoCommand { get; }

        public ICommand RemovePhotoCommand { get; }

        public ObservableCollection<PhotoItem> Photos { get; set; }

        private int _amountPhotos = 0;
        public int AmountPhotos
        {
            get => _amountPhotos;
            set => SetProperty(ref _amountPhotos, value);
        }

        public async Task PickImageAsync()
        {
            try
            {
                var result = await FilePicker.PickMultipleAsync(new PickOptions
                {
                    PickerTitle = "Выберите изображение.",
                    FileTypes = FilePickerFileType.Images,
                });

                if (result != null)
                {
                    var images = result.ToList();
                    foreach (var image in images)
                    {
                        using (var stream = await image.OpenReadAsync())
                        {
                            var memoryStream = new MemoryStream();
                            await stream.CopyToAsync(memoryStream);
                            var imageData = memoryStream.ToArray();

                            var imageSource = ImageSource.FromStream(() => new MemoryStream(imageData));

                            if (AmountPhotos == 9)
                            {
                                Photos.RemoveAt(0);
                                Photos.Insert(0, new PhotoItem() { ImageSource = imageSource });
                            }
                            else
                            {
                                Photos.RemoveAt(9);
                                Photos.Insert(1, new PhotoItem() { ImageSource = imageSource });
                            }
                            AmountPhotos++;
                        }
                    }
                }
            }
            catch (Exception) { }
        }

        public void RemovePhotoAsync(PhotoItem photoItem)
        {
            Photos.Remove(photoItem);
            Photos.Add(new());
            AmountPhotos--;
        }

        #endregion

        public async Task PublishAdvertisement()
        {
            IsLoading = true;
            for( int i = 0; i < 10; i++)
            {
                if(Photos[i].ImageSource != null)
                {
                    string imageUrl = await _imageBBRepository.Add(Photos[i].ImageSource);
                    Advertisement.ImageUrls.Add(imageUrl);
                }
            }

            await _advertisementRepository.Add(Advertisement);
            await Shell.Current.GoToAsync("//ProfilePage");

            Photos[0].IsButton = true;
            foreach(var item in Photos)
            {
                item.ImageSource = null;
            }
            AmountPhotos = 0;

            IsLoading = false;

            NeedClear = true;
        }
    }
}

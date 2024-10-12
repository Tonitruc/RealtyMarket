using CommunityToolkit.Mvvm.ComponentModel;
using RealtyMarket.Models;
using RealtyMarket.Models.RealtyEntity;
using System.Collections.ObjectModel;


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
        
        public Advertisement Advertisement { get; set; }

        #region AdvertismentParam

        private int _amountPhotos = 0;
        public int AmountPhotos
        {
            get => _amountPhotos;
            set => SetProperty(ref _amountPhotos, value);
        }

        public ObservableCollection<PhotoItem> Photos { get; private set; }

        public Command AddPhotoCommand { get; }

        public Command<PhotoItem> RemovePhotoCommand { get; }

        public ObservableCollection<string> RealtyCategories { get; } = [
                "Не выбрано",
                "Квартира",
                "Дом"
                ];

        private RealtyCategory _selectedCategory;

        public string SelectedCategory
        {
            get
            {
                if (_selectedCategory == RealtyCategory.Flat)
                    return "Квартира";
                else if (_selectedCategory == RealtyCategory.House)
                    return "Дом";

                return "Не выбрано";
            }
            set
            {
                //TODO проверить првоерку
                Enum.TryParse(value, out RealtyCategory result);
                if (result == 0)
                {
                    result = RealtyCategory.None;
                }
                SetProperty(ref _selectedCategory, result);
            }
        }

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
            "Виденаблюдение"
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

        public AddAdViewModel()
        {
            Photos = new ObservableCollection<PhotoItem>();

            Photos.Add(new PhotoItem() { IsFirst = true });

            for (int i = 1; i < 10; i++)
            {
                Photos.Add(new PhotoItem());  
            }

            AddPhotoCommand = new Command(async () => await AddPhotoAsync());
            RemovePhotoCommand = new Command<PhotoItem>(RemovePhoto);

            Advertisement = new Advertisement() { Realty = new ResidentialRealty()};
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

using RealtyMarket.Models;
using RealtyMarket.Repository;
using System.Collections.ObjectModel;
using System.Reactive.Threading.Tasks;

namespace RealtyMarket.ViewModels
{
    public class CatalogViewModel
    {
        private readonly AdvertisementRepository _advertisementRepository;

        public ObservableCollection<Advertisement> Advertisements { get; }


        public CatalogViewModel(AdvertisementRepository advertisementRepository)
        {
            _advertisementRepository = advertisementRepository;
            Advertisements = [];
        }

        public async Task GetAdvertisements()
        {
            var advertisementsList = await _advertisementRepository.GetAll();
            Advertisements.Clear();
            foreach(var ad in  advertisementsList)
            {
                Advertisements.Add(ad);
            }
        }
    }
}

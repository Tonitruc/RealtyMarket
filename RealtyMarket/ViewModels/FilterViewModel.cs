using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtyMarket.ViewModels
{
    public class FilterViewModel : ObservableObject
    {
        public ObservableCollection<string> AdvertisementTypes { get; } = [
            "Продажа",
            "Аренда"
            ];

        public ObservableCollection<string> Currencys { get; } = [
            "$", "€", "BYN", "Z$"
            ];

        public ObservableCollection<string> RealtyCategories { get; } = [
            "Не выбрано",
            "Квартира",
            "Дом"
            ];

        public ObservableCollection<string> Regions { get; } = [
            "Минск",
            "Минская",
            "Брестская",
            "Могилевская",
            "Гомельская",
            "Гродненская",
            "Витебская"
            ];

        public FilterViewModel()
        {
            
        }
    }
}

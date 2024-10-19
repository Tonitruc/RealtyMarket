using Newtonsoft.Json;
using RealtyMarket.Models.RealtyEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealtyMarket.Converters;

namespace RealtyMarket.Models
{
    public class Advertisement
    {
        public string Id { get; set; }

        private AdvertisementType _type;

        public string Type
        {
            get
            {
                if(_type == AdvertisementType.Purchase)
                {
                    return "Продажа";
                }
                else
                {
                    return "Аренда";
                }
            }
            set
            {
                if (value == "Продажа")
                {
                    _type = AdvertisementType.Purchase;
                }
                else if (value == "Аренда")
                {
                    _type = AdvertisementType.Rent;
                }
            }
        }

        public List<string> ImageUrls { get; set; } = [];

        public string Name { get; set; }

        public string SellerName { get; set; }

        public string SellerNumber { get; set; }

        private RealtyEntity.Enums.RealtyCategory _realtyCategory;

        public string RealtyCategory
        {
            get
            {
                if(_realtyCategory == RealtyEntity.Enums.RealtyCategory.Flat)
                {
                    return "Квартира";
                }
                else
                {
                    return "Дом";
                }
            }
            set
            {
                if (value == "Квартира" || value == "Flat")
                {
                    _realtyCategory = RealtyEntity.Enums.RealtyCategory.Flat;
                }
                else if(value == "Дом" || value == "PrivateHouse")
                {
                    _realtyCategory = RealtyEntity.Enums.RealtyCategory.PrivateHouse;
                }
            }
        }

        public DateTime PublishDate { get; set; }

        public Realty Realty { get; set; }

        public string UserEmail { get; set; }

        public string Description { get; set; }

        public double Cost { get; set; }

        public string Currency { get; set; }
    }
}

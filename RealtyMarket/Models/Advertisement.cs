using RealtyMarket.Models.RealtyEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtyMarket.Models
{
    public class Advertisement
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string SellerName { get; set; }

        public string SellerNumber { get; set; }

        private RealtyEntity.Enums.RealtyCategory _realtyCategory;

        public string RealtyCategory
        {
            get => _realtyCategory.ToString();
            set
            {
                if (value == "Квартира")
                {
                    _realtyCategory = RealtyEntity.Enums.RealtyCategory.Flat;
                }
                else if(value == "Дом")
                {
                    _realtyCategory = RealtyEntity.Enums.RealtyCategory.House;
                }
            }
        }

        public Realty Realty { get; set; }

        public string UserEmail { get; set; }
    }
}

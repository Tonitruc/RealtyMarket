using RealtyMarket.Models.RealtyEntity.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtyMarket.Models.RealtyEntity
{
    public class ResidentialRealty : Realty
    {
        public int AmountRooms { get; set; }

        public double LivingArea { get; set; }

        public int AmountFloors { get; set; }

        public int ConstructionYear { get; set; }

        public string CeilingHeight { get; set; }

        public List<string> Conveniences { get; set; } = [];

        public string Newness { get; set; }
    }
}

using RealtyMarket.Models.OtherEntity;
using Newtonsoft.Json;
using RealtyMarket.Converters;

namespace RealtyMarket.Models.RealtyEntity
{
    public abstract class Realty
    {
        public RealtyLocation Location { get; set; }

        public double Area { get; set; }

        public string Description { get; set; }

    }
}

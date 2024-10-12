using RealtyMarket.Models.OtherEntity;

namespace RealtyMarket.Models.RealtyEntity
{
    public abstract class Realty
    {
        public RealtyMarket.Models.OtherEntity.Location Location { get; set; }

        public double Area { get; set; }

        public string Description { get; set; }

    }
}

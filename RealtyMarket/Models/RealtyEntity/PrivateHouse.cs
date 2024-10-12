using RealtyMarket.Models.RealtyEntity.Enums;

namespace RealtyMarket.Models.RealtyEntity
{
    public class PrivateHouse : ResidentialRealty
    {
        public HouseType HouseType { get; set; }

        public int Readiness { get; set; }

        public bool HasElectricity { get; set; }

        public Water Water { get; set; }

        public SewerageSystem SewerageSystem { get; set; }

        public GasSystem GasSystem { get; set; }

        public List<string> TerritoryConveniences { get; set; }
    }
}

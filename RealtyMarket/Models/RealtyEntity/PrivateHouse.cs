using RealtyMarket.Models.RealtyEntity.Enums;

namespace RealtyMarket.Models.RealtyEntity
{
    public class PrivateHouse : ResidentialRealty
    {
        public string HouseType { get; set; }

        public bool HasElectricity { get; set; }

        public string Water { get; set; }

        public string SewerageSystem { get; set; }

        public string GasSystem { get; set; }

        public List<string> TerritoryConveniences { get; set; }
    }
}

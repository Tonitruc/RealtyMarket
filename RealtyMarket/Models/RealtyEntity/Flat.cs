using RealtyMarket.Models.RealtyEntity.Enums;

namespace RealtyMarket.Models.RealtyEntity
{
    public class Flat : ResidentialRealty
    {

        public BalconyType BalconyType { get; set; }

        public RepairType RepairType { get; set; }

        public bool IsEntranceRoom { get; set; }

        public double KitchenArea { get; set; }

        public int Floor { get; set; }

        public int FloorNumbers { get; set; }
    }
}

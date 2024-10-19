namespace RealtyMarket.Models.OtherEntity
{
    public record RealtyLocation
    {
        public double Longitude { get; init; }

        public double Latinude { get; set; }

        public string Address { get; set; }
    }
}

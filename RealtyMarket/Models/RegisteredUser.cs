namespace RealtyMarket.Models
{
    public class RegisteredUser
    {
        public string Id { get; set; } 

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Number { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string UserImageUrl { get; set; } = string.Empty;

        public List<string> Favorites { get; set; } = [];
    }
}

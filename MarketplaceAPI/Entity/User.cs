using Microsoft.AspNetCore.Identity;

namespace MarketplaceAPI.Entity
{
    public class User : IdentityUser
    {
        public string State { get; set; }
        public string City { get; set; }
        public string? PhotoUrl { get; set; }
        public List<Product> Products { get; set; } = new();
    }
}

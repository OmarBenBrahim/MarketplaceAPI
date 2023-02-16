using MarketplaceAPI.Entity;

namespace MarketplaceAPI.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(User user);
    }
}

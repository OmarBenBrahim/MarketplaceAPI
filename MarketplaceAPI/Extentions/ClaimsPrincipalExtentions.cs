using System.Security.Claims;

namespace MarketplaceAPI.Extentions
{
    public static class ClaimsPrincipalExtentions
    {
        public static string GetUserame(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Name)?.Value;
        }

        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}

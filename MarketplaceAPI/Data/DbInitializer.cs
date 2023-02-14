using MarketplaceAPI.Entity;
using System.Text.Json;

namespace MarketplaceAPI.Data
{
    public class DbInitializer
    {
        public static async Task Initialize(DataContext context) 
        {
            if (!context.Products.Any()) {
                var ProductData = await File.ReadAllTextAsync("Data/ProductsSeedData.json");
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                List<Product> products = JsonSerializer.Deserialize<List<Product>>(ProductData);
                context.Products.AddRange(products);
                await context.SaveChangesAsync();
            }
        }
    }
}

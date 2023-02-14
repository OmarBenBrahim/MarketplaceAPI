using MarketplaceAPI.Entity;
using Microsoft.EntityFrameworkCore;

namespace MarketplaceAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Categorie> Categories { get; set; }
        public DbSet<Photo> Photos { get; set; }


        }
}

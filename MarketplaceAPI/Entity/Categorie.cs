namespace MarketplaceAPI.Entity
{
    public class Categorie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Product> Products { get; set; }

    }
}

namespace MarketplaceAPI.DTOs
{
    public class CreateProductDto
    {
        public string Title { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public string Categorie { get; set; }
    }
}

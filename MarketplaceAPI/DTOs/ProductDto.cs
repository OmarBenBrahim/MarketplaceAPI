using MarketplaceAPI.Entity;

namespace MarketplaceAPI.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public string CategorieName { get; set; }
        public List<PhotoDto> Photos { get; set; }
        public ProductUserDto? User { get; set; }

    }
}

namespace MarketplaceAPI.Helpers
{
    public class ProductParams
    {
        public float? MaxPrice { get; set; }
        public float? MinPrice { get; set; }
        public string? Categorie { get; set; }
        public string? State { get; set; }
        public string? UserName { get; set; }
        public int pageNumber { get; set; } = 1;
        public int pageSize { get; set; } = 8;
    }
}

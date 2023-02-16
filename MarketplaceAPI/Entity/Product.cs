namespace MarketplaceAPI.Entity;
using System.ComponentModel.DataAnnotations;

public class Product
{
    public int Id { get; set; }
    public string Title { get; set; }
    public float Price { get; set; }
    public string Description { get; set; }
    public DateTime Created { get; set; }
    public List<Photo> Photos { get; set; } = new();
    public int CategorieId { get; set; }
    public Categorie Categorie { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }

}

using backend.Models;
using backend.Models.Enums;

namespace backend.Models;

public class Product : BaseEntity
{
    public string Name { get; set; } = "";
    public string ShortDescription { get; set; } = "";
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public ProductType ProductType { get; set; }
}
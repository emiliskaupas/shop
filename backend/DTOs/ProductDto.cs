using backend.Models.Enums;

namespace backend.DTOs;

public class ProductDto
{
    public long Id { get; set; }
    public string Name { get; set; } = "";
    public string ShortDescription { get; set; } = "";
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public ProductType ProductType { get; set; }
    
    // Creator information
    public long CreatedByUserId { get; set; }
    public string CreatedByUserName { get; set; } = "";
    public string CreatedByEmail { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}

public class CreateProductDto
{
    public string Name { get; set; } = "";
    public string ShortDescription { get; set; } = "";
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public ProductType ProductType { get; set; }
}

public class UpdateProductDto
{
    public string Name { get; set; } = "";
    public string ShortDescription { get; set; } = "";
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public ProductType ProductType { get; set; }
}
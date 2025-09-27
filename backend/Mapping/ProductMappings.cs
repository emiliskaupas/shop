using backend.DTOs;
using backend.Models;

namespace backend.Mapping;

public static class ProductMappings
{
    public static ProductDto ToDto(this Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            ShortDescription = product.ShortDescription,
            Price = product.Price,
            ImageUrl = product.ImageUrl,
            ProductType = product.ProductType
        };
    }

    public static Product ToEntity(this CreateProductDto dto)
    {
        return new Product
        {
            Name = dto.Name,
            ShortDescription = dto.ShortDescription,
            Price = dto.Price,
            ImageUrl = dto.ImageUrl,
            ProductType = dto.ProductType
        };
    }

    public static void UpdateEntity(this UpdateProductDto dto, Product product)
    {
        product.Name = dto.Name;
        product.ShortDescription = dto.ShortDescription;
        product.Price = dto.Price;
        product.ImageUrl = dto.ImageUrl;
        product.ProductType = dto.ProductType;
    }
}
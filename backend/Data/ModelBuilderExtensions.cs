namespace backend.Data;
using backend.Models;
using backend.Models.Enums;
using Microsoft.EntityFrameworkCore;
public static class ModelBuilderExtensions
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 1,
                Name = "iPhone 15 Pro",
                ShortDescription = "Latest iPhone with Pro features and titanium design.",
                Price = 999.99M,
                ImageUrl = "https://example.com/iphone15pro.jpg",
                ProductType = ProductType.Electronics
            },
            new Product
            {
                Id = 2,
                Name = "Nike Air Max",
                ShortDescription = "Comfortable running shoes with Air Max technology.",
                Price = 129.99M,
                ImageUrl = "https://example.com/nike-airmax.jpg", 
                ProductType = ProductType.Clothing
            },
            new Product
            {
                Id = 3,
                Name = "MacBook Pro 14\"",
                ShortDescription = "Powerful laptop with M3 chip for professional work.",
                Price = 1999.99M,
                ImageUrl = "https://example.com/macbook-pro.jpg",
                ProductType = ProductType.Electronics
            },
            new Product
            {
                Id = 4,
                Name = "Levi's 501 Jeans",
                ShortDescription = "Classic straight-fit jeans in premium denim.",
                Price = 79.99M,
                ImageUrl = "https://example.com/levis-501.jpg",
                ProductType = ProductType.Clothing
            },
            new Product
            {
                Id = 5,
                Name = "Samsung 4K TV",
                ShortDescription = "55-inch 4K Smart TV with HDR and streaming apps.",
                Price = 599.99M,
                ImageUrl = "https://example.com/samsung-tv.jpg",
                ProductType = ProductType.Electronics
            }
        );

        modelBuilder.Entity<User>().HasData(
            // Admin user
            new User
            {
                Id = 1,
                Username = "admin",
                Email = "admin@shop.com",
                PasswordHash = "admin123", // TODO: Hash this password in production
                Role = UserRole.Admin
            },
            // Customer user for testing
            new User
            {
                Id = 2,
                Username = "john_doe",
                Email = "john.doe@example.com", 
                PasswordHash = "customer123", // TODO: Hash this password in production
                Role = UserRole.Customer
            },
            // Another customer for testing
            new User
            {
                Id = 3,
                Username = "jane_smith",
                Email = "jane.smith@example.com",
                PasswordHash = "customer123", // TODO: Hash this password in production
                Role = UserRole.Customer
            }
        );

        modelBuilder.Entity<CartItem>().HasData(
            // John's cart items
            new CartItem
            {
                Id = 1,
                UserId = 2, // john_doe
                ProductId = 1, // iPhone 15 Pro
                Quantity = 1
            },
            new CartItem
            {
                Id = 2,
                UserId = 2, // john_doe
                ProductId = 2, // Nike Air Max
                Quantity = 2
            },
            new CartItem
            {
                Id = 3,
                UserId = 2, // john_doe
                ProductId = 4, // Levi's 501 Jeans
                Quantity = 1
            },
            // Jane's cart items
            new CartItem
            {
                Id = 4,
                UserId = 3, // jane_smith
                ProductId = 3, // MacBook Pro 14"
                Quantity = 1
            },
            new CartItem
            {
                Id = 5,
                UserId = 3, // jane_smith
                ProductId = 5, // Samsung 4K TV
                Quantity = 1
            }
        );
    }
}

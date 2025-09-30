using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using backend.Services;
using backend.Data;
using backend.Models;
using backend.DTOs;
using backend.Models.Enums;

namespace backend.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly AppDbContext dbContext;
        private readonly ProductService service;

        public ProductServiceTests()
        {
            // Use a unique InMemory database for each test class
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            dbContext = new AppDbContext(options);

            // Seed users
            dbContext.Users.AddRange(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    Email = "admin@shop.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    Role = UserRole.Admin
                },
                new User
                {
                    Id = 2,
                    Username = "john_doe",
                    Email = "john.doe@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("customer123"),
                    Role = UserRole.Customer
                }
            );

            // Seed products
            dbContext.Products.AddRange(
                new Product
                {
                    Id = 1,
                    Name = "Test Product",
                    Price = 10,
                    CreatedByUserId = 1,
                    CreatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Id = 2,
                    Name = "Other User Product",
                    Price = 15,
                    CreatedByUserId = 2,
                    CreatedAt = DateTime.UtcNow
                }
            );

            dbContext.SaveChanges();

            service = new ProductService(dbContext);
        }

        [Fact]
        public async Task GetProductByIdAsync_Returns_Product_When_Found()
        {
            var product = dbContext.Products.First();
            var result = await service.GetProductByIdAsync(product.Id);

            Assert.True(result.IsSuccess);
            Assert.Equal("Test Product", result.Data!.Name);
        }

        [Fact]
        public async Task GetProductByIdAsync_Returns_Failure_When_NotFound()
        {
            var result = await service.GetProductByIdAsync(999);

            Assert.False(result.IsSuccess);
            Assert.Equal("Product not found", result.ErrorMessage);
        }

        [Fact]
        public async Task AddProductAsync_Creates_Product_Successfully()
        {
            var newProductDto = new CreateProductDto
            {
                Name = "New Product",
                Price = 20
            };

            var result = await service.AddProductAsync(newProductDto, 1); // admin user

            Assert.True(result.IsSuccess);
            Assert.Equal("New Product", result.Data!.Name);
            Assert.Equal(20, result.Data.Price);
        }

        [Fact]
        public async Task UpdateProductAsync_Updates_Product_Successfully()
        {
            var product = dbContext.Products.First();
            var updateDto = new UpdateProductDto
            {
                Name = "Updated Product",
                Price = 50
            };

            var result = await service.UpdateProductAsync(product.Id, updateDto, product.CreatedByUserId);

            Assert.True(result.IsSuccess);
            Assert.Equal("Updated Product", result.Data!.Name);
            Assert.Equal(50, result.Data.Price);
        }

        [Fact]
        public async Task DeleteProductAsync_Removes_Product_Successfully()
        {
            var product = dbContext.Products.First();

            var result = await service.DeleteProductAsync(product.Id, product.CreatedByUserId);

            Assert.True(result.IsSuccess);

            var check = await service.GetProductByIdAsync(product.Id);
            Assert.False(check.IsSuccess);
        }

        [Fact]
        public async Task GetProductsByUserAsync_Filters_By_User()
        {
            var request = new Shop.Shared.Pagination.PaginationRequest { Page = 1, PageSize = 10 };
            var result = await service.GetProductsByUserAsync(1, request);

            Assert.True(result.IsSuccess);
            Assert.Single(result.Data!.Items);
            Assert.Equal("Test Product", result.Data.Items.First().Name);
        }
    }
}
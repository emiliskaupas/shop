using Microsoft.EntityFrameworkCore;
using Backend.Models;
namespace Backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }
    public DbSet<Product> Products => Set<Product>();

    // Seed
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Chair", ShortDescription = "Simple chair", Price = 49.99m, ImageUrl = null },
            new Product { Id = 2, Name = "Table", ShortDescription = "Small table", Price = 99.99m, ImageUrl = null }
        );
    }
}

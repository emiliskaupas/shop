using Microsoft.EntityFrameworkCore;
using backend.Models;
using backend.Models.Enums;
namespace backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }
    
    public DbSet<Product> Products => Set<Product>();
    public DbSet<User> Users => Set<User>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure User -> CartItems relationship (One-to-Many)
        modelBuilder.Entity<User>()
            .HasMany(u => u.CartItems)
            .WithOne()
            .HasForeignKey(ci => ci.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure Product -> CartItems relationship (One-to-Many)
        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.Product)
            .WithMany()
            .HasForeignKey(ci => ci.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Seed();
    }
}

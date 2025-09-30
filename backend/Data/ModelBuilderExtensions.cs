namespace backend.Data;
using backend.Models;
using backend.Models.Enums;
using Microsoft.EntityFrameworkCore;
public static class ModelBuilderExtensions
{
    public static void ConfigureEntities(this ModelBuilder modelBuilder)
    {
        // Configure Product entity
        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);
                
            entity.Property(p => p.ShortDescription)
                .HasMaxLength(500);
                
            entity.Property(p => p.Price)
                .HasColumnType("decimal(18,2)");
                
            entity.Property(p => p.CreatedAt)
                .IsRequired();
                
            entity.Property(p => p.ModifiedAt);
        });
        
        // Configure User entity
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);
                
            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);
                
            entity.HasIndex(u => u.Username)
                .IsUnique();
                
            entity.HasIndex(u => u.Email)
                .IsUnique();
        });

        // Configure CartItem entity and relationships
        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.Property(ci => ci.Quantity)
                .IsRequired();

            // Configure relationship with Product - CASCADE DELETE
            entity.HasOne(ci => ci.Product)
                .WithMany()
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Don't configure the User relationship here since we don't have a navigation property
        });

        // Configure Product-User relationship for CreatedBy
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasOne(p => p.CreatedBy)
                .WithMany()
                .HasForeignKey(p => p.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict); // Don't allow deleting users who created products
        });
    }

    public static void Seed(this ModelBuilder modelBuilder)
    {
        var adminUserId = 1; // Admin user ID
        var validImageUrl = "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400"; // Icon-based placeholder service

        modelBuilder.Entity<Product>().HasData(
            // Electronics (20 products)
            new Product
            {
                Id = 1,
                Name = "iPhone 15 Pro Max",
                ShortDescription = "Latest iPhone with Pro features and titanium design.",
                Price = 1199.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Electronics,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 1, 10, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 2,
                Name = "Samsung Galaxy S24 Ultra",
                ShortDescription = "Premium Android phone with S Pen and incredible camera.",
                Price = 1099.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Electronics,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 1, 11, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 3,
                Name = "MacBook Pro 16\" M3",
                ShortDescription = "Powerful laptop with M3 Max chip for professional work.",
                Price = 2499.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Electronics,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 1, 12, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 4,
                Name = "Dell XPS 13 Plus",
                ShortDescription = "Ultra-portable Windows laptop with premium design.",
                Price = 1299.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Electronics,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 1, 13, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 5,
                Name = "iPad Pro 12.9\" M2",
                ShortDescription = "Professional tablet with M2 chip and Apple Pencil support.",
                Price = 1099.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Electronics,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 1, 14, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 6,
                Name = "Sony WH-1000XM5 Headphones",
                ShortDescription = "Industry-leading noise canceling wireless headphones.",
                Price = 349.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Electronics,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 1, 15, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 7,
                Name = "Apple Watch Series 9",
                ShortDescription = "Advanced health tracking and connectivity features.",
                Price = 399.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Electronics,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 1, 16, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 8,
                Name = "Nintendo Switch OLED",
                ShortDescription = "Portable gaming console with vibrant OLED screen.",
                Price = 349.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Electronics,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 2, 9, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 9,
                Name = "PlayStation 5",
                ShortDescription = "Next-gen gaming console with lightning-fast loading.",
                Price = 499.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Electronics,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 2, 10, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 10,
                Name = "Xbox Series X",
                ShortDescription = "Most powerful Xbox ever with 4K gaming.",
                Price = 499.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Electronics,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 2, 11, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 11,
                Name = "Canon EOS R5",
                ShortDescription = "Professional mirrorless camera with 8K video recording.",
                Price = 3899.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Electronics,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 2, 12, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 12,
                Name = "Sony A7 IV",
                ShortDescription = "Full-frame mirrorless camera for content creators.",
                Price = 2498.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Electronics,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 2, 13, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 13,
                Name = "Samsung 65\" QLED 4K TV",
                ShortDescription = "Premium smart TV with Quantum Dot technology.",
                Price = 1299.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Electronics,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 2, 14, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 14,
                Name = "LG OLED C3 55\"",
                ShortDescription = "Perfect blacks and infinite contrast with OLED technology.",
                Price = 1799.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Electronics,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 2, 15, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 15,
                Name = "Bose SoundLink Revolve+",
                ShortDescription = "360-degree portable Bluetooth speaker.",
                Price = 299.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Electronics,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 2, 16, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 16,
                Name = "Apple AirPods Pro 2",
                ShortDescription = "Active noise cancellation with spatial audio.",
                Price = 249.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Electronics,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 3, 9, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 17,
                Name = "Samsung Galaxy Buds2 Pro",
                ShortDescription = "Premium wireless earbuds with intelligent ANC.",
                Price = 199.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Electronics,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 3, 10, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 18,
                Name = "Microsoft Surface Pro 9",
                ShortDescription = "Versatile 2-in-1 laptop and tablet hybrid.",
                Price = 1099.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Electronics,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 3, 11, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 19,
                Name = "Fitbit Sense 2",
                ShortDescription = "Advanced health and fitness smartwatch.",
                Price = 299.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Electronics,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 3, 12, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 20,
                Name = "Google Pixel 8 Pro",
                ShortDescription = "AI-powered camera with computational photography.",
                Price = 999.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Electronics,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 3, 13, 0, 0, DateTimeKind.Utc)
            },

            // Clothing (15 products)
            new Product
            {
                Id = 21,
                Name = "Nike Air Max 270",
                ShortDescription = "Lifestyle sneakers with maximum Air cushioning.",
                Price = 149.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Clothing,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 3, 14, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 22,
                Name = "Adidas Ultraboost 23",
                ShortDescription = "Premium running shoes with responsive cushioning.",
                Price = 189.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Clothing,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 3, 15, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 23,
                Name = "Levi's 511 Slim Jeans",
                ShortDescription = "Classic slim-fit jeans in premium stretch denim.",
                Price = 89.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Clothing,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 3, 16, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 24,
                Name = "Ralph Lauren Polo Shirt",
                ShortDescription = "Classic cotton polo with iconic pony logo.",
                Price = 89.50M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Clothing,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 4, 9, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 25,
                Name = "Champion Reverse Weave Hoodie",
                ShortDescription = "Premium heavyweight hoodie with iconic logo.",
                Price = 79.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Clothing,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 4, 10, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 26,
                Name = "Patagonia Down Jacket",
                ShortDescription = "Lightweight insulation for outdoor adventures.",
                Price = 229.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Clothing,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 4, 11, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 27,
                Name = "Uniqlo Heattech Long Sleeve",
                ShortDescription = "Ultra-warm base layer with moisture-wicking technology.",
                Price = 19.90M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Clothing,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 4, 12, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 28,
                Name = "Zara Wool Blend Coat",
                ShortDescription = "Elegant winter coat with tailored fit.",
                Price = 199.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Clothing,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 4, 13, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 29,
                Name = "Under Armour Athletic Shorts",
                ShortDescription = "Moisture-wicking shorts for intense workouts.",
                Price = 39.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Clothing,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 4, 14, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 30,
                Name = "Lululemon Align Leggings",
                ShortDescription = "Buttery-soft yoga leggings with four-way stretch.",
                Price = 128.00M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Clothing,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 4, 15, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 31,
                Name = "H&M Organic Cotton T-Shirt",
                ShortDescription = "Sustainable basic tee in soft organic cotton.",
                Price = 12.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Clothing,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 4, 16, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 32,
                Name = "Calvin Klein Underwear Set",
                ShortDescription = "Premium cotton blend underwear for all-day comfort.",
                Price = 49.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Clothing,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 5, 9, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 33,
                Name = "Converse Chuck Taylor All Star",
                ShortDescription = "Iconic canvas sneakers for casual style.",
                Price = 65.00M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Clothing,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 5, 10, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 34,
                Name = "Tommy Hilfiger Button-Down Shirt",
                ShortDescription = "Classic dress shirt with signature flag logo.",
                Price = 79.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Clothing,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 5, 11, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 35,
                Name = "Ray-Ban Aviator Sunglasses",
                ShortDescription = "Timeless aviator style with UV protection.",
                Price = 159.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Clothing,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 5, 12, 0, 0, DateTimeKind.Utc)
            },

            // Home Appliances (8 products)
            new Product
            {
                Id = 36,
                Name = "Dyson V15 Detect Vacuum",
                ShortDescription = "Powerful cordless vacuum with laser detection.",
                Price = 749.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.HomeAppliances,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 5, 13, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 37,
                Name = "KitchenAid Stand Mixer",
                ShortDescription = "Professional-grade mixer for all your baking needs.",
                Price = 449.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.HomeAppliances,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 5, 14, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 38,
                Name = "Nespresso Vertuo Plus",
                ShortDescription = "Premium coffee machine with barista-quality results.",
                Price = 199.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.HomeAppliances,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 5, 15, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 39,
                Name = "Instant Pot Duo 7-in-1",
                ShortDescription = "Multi-functional pressure cooker for quick meals.",
                Price = 99.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.HomeAppliances,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 5, 16, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 40,
                Name = "Roomba i7+ Robot Vacuum",
                ShortDescription = "Smart vacuum that empties itself for weeks.",
                Price = 799.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.HomeAppliances,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 6, 9, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 41,
                Name = "Air Fryer Pro XL",
                ShortDescription = "Large capacity air fryer for healthy cooking.",
                Price = 159.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.HomeAppliances,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 6, 10, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 42,
                Name = "Vitamix Professional Blender",
                ShortDescription = "High-performance blender for smoothies and more.",
                Price = 549.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.HomeAppliances,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 6, 11, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 43,
                Name = "Nest Learning Thermostat",
                ShortDescription = "Smart thermostat that learns your schedule.",
                Price = 249.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.HomeAppliances,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 6, 12, 0, 0, DateTimeKind.Utc)
            },

            // Books (6 products)
            new Product
            {
                Id = 44,
                Name = "The Psychology of Money",
                ShortDescription = "Timeless lessons on wealth, greed, and happiness.",
                Price = 24.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Books,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 6, 13, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 45,
                Name = "Atomic Habits",
                ShortDescription = "An easy and proven way to build good habits.",
                Price = 18.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Books,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 6, 14, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 46,
                Name = "Clean Code",
                ShortDescription = "A handbook of agile software craftsmanship.",
                Price = 49.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Books,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 6, 15, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 47,
                Name = "The Midnight Library",
                ShortDescription = "A novel about all the choices that go into a life.",
                Price = 16.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Books,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 6, 16, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 48,
                Name = "Sapiens: A Brief History",
                ShortDescription = "From animals into gods: a brief history of humankind.",
                Price = 22.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Books,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 7, 9, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 49,
                Name = "The 7 Habits of Highly Effective People",
                ShortDescription = "Powerful lessons in personal change.",
                Price = 19.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Books,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 7, 10, 0, 0, DateTimeKind.Utc)
            },

            // Toys (3 products)
            new Product
            {
                Id = 50,
                Name = "LEGO Creator Expert Set",
                ShortDescription = "Advanced building set for adult LEGO enthusiasts.",
                Price = 199.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Toys,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 7, 11, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 51,
                Name = "Nintendo Switch Pro Controller",
                ShortDescription = "Premium gaming controller with enhanced precision.",
                Price = 69.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Toys,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 7, 12, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 52,
                Name = "Rubik's Cube Speed Cube",
                ShortDescription = "Professional speed cube for competitive solving.",
                Price = 19.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Toys,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 7, 13, 0, 0, DateTimeKind.Utc)
            },

            // Sports Equipment (3 products)
            new Product
            {
                Id = 53,
                Name = "Peloton Bike+",
                ShortDescription = "Premium exercise bike with live and on-demand classes.",
                Price = 2495.00M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.SportsEquipment,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 7, 14, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 54,
                Name = "Adjustable Dumbbell Set",
                ShortDescription = "Space-saving dumbbells that replace multiple weights.",
                Price = 399.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.SportsEquipment,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 7, 15, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 55,
                Name = "Yoga Mat Premium",
                ShortDescription = "Non-slip yoga mat with superior cushioning.",
                Price = 89.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.SportsEquipment,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 7, 16, 0, 0, DateTimeKind.Utc)
            },

            // Beauty Products (2 products)
            new Product
            {
                Id = 56,
                Name = "Dyson Airwrap Complete",
                ShortDescription = "Multi-styler for all hair types with no extreme heat.",
                Price = 599.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.BeautyProducts,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 8, 9, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 57,
                Name = "Skincare Routine Set",
                ShortDescription = "Complete skincare system for healthy, glowing skin.",
                Price = 149.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.BeautyProducts,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 8, 10, 0, 0, DateTimeKind.Utc)
            },

            // Furniture (2 products)
            new Product
            {
                Id = 58,
                Name = "Herman Miller Aeron Chair",
                ShortDescription = "Ergonomic office chair for all-day comfort.",
                Price = 1395.00M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Furniture,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 8, 11, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 59,
                Name = "Standing Desk Converter",
                ShortDescription = "Transform any desk into a healthy standing workspace.",
                Price = 299.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Furniture,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 8, 12, 0, 0, DateTimeKind.Utc)
            },

            // Automotive (1 product)
            new Product
            {
                Id = 60,
                Name = "Tesla Model Y Accessories Kit",
                ShortDescription = "Premium accessories bundle for Tesla Model Y.",
                Price = 499.99M,
                ImageUrl = validImageUrl,
                ProductType = ProductType.Automotive,
                CreatedByUserId = adminUserId,
                CreatedAt = new DateTime(2025, 9, 8, 13, 0, 0, DateTimeKind.Utc)
            }
        );

        modelBuilder.Entity<User>().HasData(
            // Admin user
            new User
            {
                Id = 1,
                Username = "admin",
                Email = "admin@shop.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Role = UserRole.Admin
            },
            // Customer user for testing
            new User
            {
                Id = 2,
                Username = "john_doe",
                Email = "john.doe@example.com", 
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("customer123"),
                Role = UserRole.Customer
            },
            // Another customer for testing
            new User
            {
                Id = 3,
                Username = "jane_smith",
                Email = "jane.smith@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("customer123"),
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

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductType = table.Column<int>(type: "int", nullable: false),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { 1L, "admin@shop.com", "$2a$11$3BwaI6Drhh4YBps1lDQ67urWqnNfO6tdicamrUE.ebzpW/MmSJa.O", 1, "admin" },
                    { 2L, "john.doe@example.com", "$2a$11$W0xDb3m/9c8YocmkCrC0beWWaTexoQctvM0BMl0x7fpRsUO6rC8ru", 0, "john_doe" },
                    { 3L, "jane.smith@example.com", "$2a$11$y5A/zu1/W6hcmBsgoIKqAOdqN9cnkD78O5hAJMBwRy2VHV/89jUe6", 0, "jane_smith" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CreatedAt", "CreatedByUserId", "ImageUrl", "ModifiedAt", "Name", "Price", "ProductType", "ShortDescription" },
                values: new object[,]
                {
                    { 1L, new DateTime(2025, 9, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "iPhone 15 Pro Max", 1199.99m, 0, "Latest iPhone with Pro features and titanium design." },
                    { 2L, new DateTime(2025, 9, 1, 11, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Samsung Galaxy S24 Ultra", 1099.99m, 0, "Premium Android phone with S Pen and incredible camera." },
                    { 3L, new DateTime(2025, 9, 1, 12, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "MacBook Pro 16\" M3", 2499.99m, 0, "Powerful laptop with M3 Max chip for professional work." },
                    { 4L, new DateTime(2025, 9, 1, 13, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Dell XPS 13 Plus", 1299.99m, 0, "Ultra-portable Windows laptop with premium design." },
                    { 5L, new DateTime(2025, 9, 1, 14, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "iPad Pro 12.9\" M2", 1099.99m, 0, "Professional tablet with M2 chip and Apple Pencil support." },
                    { 6L, new DateTime(2025, 9, 1, 15, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Sony WH-1000XM5 Headphones", 349.99m, 0, "Industry-leading noise canceling wireless headphones." },
                    { 7L, new DateTime(2025, 9, 1, 16, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Apple Watch Series 9", 399.99m, 0, "Advanced health tracking and connectivity features." },
                    { 8L, new DateTime(2025, 9, 2, 9, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Nintendo Switch OLED", 349.99m, 0, "Portable gaming console with vibrant OLED screen." },
                    { 9L, new DateTime(2025, 9, 2, 10, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "PlayStation 5", 499.99m, 0, "Next-gen gaming console with lightning-fast loading." },
                    { 10L, new DateTime(2025, 9, 2, 11, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Xbox Series X", 499.99m, 0, "Most powerful Xbox ever with 4K gaming." },
                    { 11L, new DateTime(2025, 9, 2, 12, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Canon EOS R5", 3899.99m, 0, "Professional mirrorless camera with 8K video recording." },
                    { 12L, new DateTime(2025, 9, 2, 13, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Sony A7 IV", 2498.99m, 0, "Full-frame mirrorless camera for content creators." },
                    { 13L, new DateTime(2025, 9, 2, 14, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Samsung 65\" QLED 4K TV", 1299.99m, 0, "Premium smart TV with Quantum Dot technology." },
                    { 14L, new DateTime(2025, 9, 2, 15, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "LG OLED C3 55\"", 1799.99m, 0, "Perfect blacks and infinite contrast with OLED technology." },
                    { 15L, new DateTime(2025, 9, 2, 16, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Bose SoundLink Revolve+", 299.99m, 0, "360-degree portable Bluetooth speaker." },
                    { 16L, new DateTime(2025, 9, 3, 9, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Apple AirPods Pro 2", 249.99m, 0, "Active noise cancellation with spatial audio." },
                    { 17L, new DateTime(2025, 9, 3, 10, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Samsung Galaxy Buds2 Pro", 199.99m, 0, "Premium wireless earbuds with intelligent ANC." },
                    { 18L, new DateTime(2025, 9, 3, 11, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Microsoft Surface Pro 9", 1099.99m, 0, "Versatile 2-in-1 laptop and tablet hybrid." },
                    { 19L, new DateTime(2025, 9, 3, 12, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Fitbit Sense 2", 299.99m, 0, "Advanced health and fitness smartwatch." },
                    { 20L, new DateTime(2025, 9, 3, 13, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Google Pixel 8 Pro", 999.99m, 0, "AI-powered camera with computational photography." },
                    { 21L, new DateTime(2025, 9, 3, 14, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Nike Air Max 270", 149.99m, 1, "Lifestyle sneakers with maximum Air cushioning." },
                    { 22L, new DateTime(2025, 9, 3, 15, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Adidas Ultraboost 23", 189.99m, 1, "Premium running shoes with responsive cushioning." },
                    { 23L, new DateTime(2025, 9, 3, 16, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Levi's 511 Slim Jeans", 89.99m, 1, "Classic slim-fit jeans in premium stretch denim." },
                    { 24L, new DateTime(2025, 9, 4, 9, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Ralph Lauren Polo Shirt", 89.50m, 1, "Classic cotton polo with iconic pony logo." },
                    { 25L, new DateTime(2025, 9, 4, 10, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Champion Reverse Weave Hoodie", 79.99m, 1, "Premium heavyweight hoodie with iconic logo." },
                    { 26L, new DateTime(2025, 9, 4, 11, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Patagonia Down Jacket", 229.99m, 1, "Lightweight insulation for outdoor adventures." },
                    { 27L, new DateTime(2025, 9, 4, 12, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Uniqlo Heattech Long Sleeve", 19.90m, 1, "Ultra-warm base layer with moisture-wicking technology." },
                    { 28L, new DateTime(2025, 9, 4, 13, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Zara Wool Blend Coat", 199.99m, 1, "Elegant winter coat with tailored fit." },
                    { 29L, new DateTime(2025, 9, 4, 14, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Under Armour Athletic Shorts", 39.99m, 1, "Moisture-wicking shorts for intense workouts." },
                    { 30L, new DateTime(2025, 9, 4, 15, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Lululemon Align Leggings", 128.00m, 1, "Buttery-soft yoga leggings with four-way stretch." },
                    { 31L, new DateTime(2025, 9, 4, 16, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "H&M Organic Cotton T-Shirt", 12.99m, 1, "Sustainable basic tee in soft organic cotton." },
                    { 32L, new DateTime(2025, 9, 5, 9, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Calvin Klein Underwear Set", 49.99m, 1, "Premium cotton blend underwear for all-day comfort." },
                    { 33L, new DateTime(2025, 9, 5, 10, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Converse Chuck Taylor All Star", 65.00m, 1, "Iconic canvas sneakers for casual style." },
                    { 34L, new DateTime(2025, 9, 5, 11, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Tommy Hilfiger Button-Down Shirt", 79.99m, 1, "Classic dress shirt with signature flag logo." },
                    { 35L, new DateTime(2025, 9, 5, 12, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Ray-Ban Aviator Sunglasses", 159.99m, 1, "Timeless aviator style with UV protection." },
                    { 36L, new DateTime(2025, 9, 5, 13, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Dyson V15 Detect Vacuum", 749.99m, 2, "Powerful cordless vacuum with laser detection." },
                    { 37L, new DateTime(2025, 9, 5, 14, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "KitchenAid Stand Mixer", 449.99m, 2, "Professional-grade mixer for all your baking needs." },
                    { 38L, new DateTime(2025, 9, 5, 15, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Nespresso Vertuo Plus", 199.99m, 2, "Premium coffee machine with barista-quality results." },
                    { 39L, new DateTime(2025, 9, 5, 16, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Instant Pot Duo 7-in-1", 99.99m, 2, "Multi-functional pressure cooker for quick meals." },
                    { 40L, new DateTime(2025, 9, 6, 9, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Roomba i7+ Robot Vacuum", 799.99m, 2, "Smart vacuum that empties itself for weeks." },
                    { 41L, new DateTime(2025, 9, 6, 10, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Air Fryer Pro XL", 159.99m, 2, "Large capacity air fryer for healthy cooking." },
                    { 42L, new DateTime(2025, 9, 6, 11, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Vitamix Professional Blender", 549.99m, 2, "High-performance blender for smoothies and more." },
                    { 43L, new DateTime(2025, 9, 6, 12, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Nest Learning Thermostat", 249.99m, 2, "Smart thermostat that learns your schedule." },
                    { 44L, new DateTime(2025, 9, 6, 13, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "The Psychology of Money", 24.99m, 3, "Timeless lessons on wealth, greed, and happiness." },
                    { 45L, new DateTime(2025, 9, 6, 14, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Atomic Habits", 18.99m, 3, "An easy and proven way to build good habits." },
                    { 46L, new DateTime(2025, 9, 6, 15, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Clean Code", 49.99m, 3, "A handbook of agile software craftsmanship." },
                    { 47L, new DateTime(2025, 9, 6, 16, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "The Midnight Library", 16.99m, 3, "A novel about all the choices that go into a life." },
                    { 48L, new DateTime(2025, 9, 7, 9, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Sapiens: A Brief History", 22.99m, 3, "From animals into gods: a brief history of humankind." },
                    { 49L, new DateTime(2025, 9, 7, 10, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "The 7 Habits of Highly Effective People", 19.99m, 3, "Powerful lessons in personal change." },
                    { 50L, new DateTime(2025, 9, 7, 11, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "LEGO Creator Expert Set", 199.99m, 4, "Advanced building set for adult LEGO enthusiasts." },
                    { 51L, new DateTime(2025, 9, 7, 12, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Nintendo Switch Pro Controller", 69.99m, 4, "Premium gaming controller with enhanced precision." },
                    { 52L, new DateTime(2025, 9, 7, 13, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Rubik's Cube Speed Cube", 19.99m, 4, "Professional speed cube for competitive solving." },
                    { 53L, new DateTime(2025, 9, 7, 14, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Peloton Bike+", 2495.00m, 5, "Premium exercise bike with live and on-demand classes." },
                    { 54L, new DateTime(2025, 9, 7, 15, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Adjustable Dumbbell Set", 399.99m, 5, "Space-saving dumbbells that replace multiple weights." },
                    { 55L, new DateTime(2025, 9, 7, 16, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Yoga Mat Premium", 89.99m, 5, "Non-slip yoga mat with superior cushioning." },
                    { 56L, new DateTime(2025, 9, 8, 9, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Dyson Airwrap Complete", 599.99m, 6, "Multi-styler for all hair types with no extreme heat." },
                    { 57L, new DateTime(2025, 9, 8, 10, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Skincare Routine Set", 149.99m, 6, "Complete skincare system for healthy, glowing skin." },
                    { 58L, new DateTime(2025, 9, 8, 11, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Herman Miller Aeron Chair", 1395.00m, 8, "Ergonomic office chair for all-day comfort." },
                    { 59L, new DateTime(2025, 9, 8, 12, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Standing Desk Converter", 299.99m, 8, "Transform any desk into a healthy standing workspace." },
                    { 60L, new DateTime(2025, 9, 8, 13, 0, 0, 0, DateTimeKind.Utc), 1L, "https://api.iconify.design/mdi:package-variant.svg?color=%23666666&width=400&height=400", null, "Tesla Model Y Accessories Kit", 499.99m, 9, "Premium accessories bundle for Tesla Model Y." }
                });

            migrationBuilder.InsertData(
                table: "CartItems",
                columns: new[] { "Id", "ProductId", "Quantity", "UserId" },
                values: new object[,]
                {
                    { 1L, 1L, 1, 2L },
                    { 2L, 2L, 2, 2L },
                    { 3L, 4L, 1, 2L },
                    { 4L, 3L, 1, 3L },
                    { 5L, 5L, 1, 3L }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_UserId",
                table: "CartItems",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CreatedByUserId",
                table: "Products",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateWithRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ShortDescription = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    ProductType = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<long>(type: "INTEGER", nullable: false),
                    ProductId = table.Column<long>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CartItems_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "ImageUrl", "Name", "Price", "ProductType", "ShortDescription" },
                values: new object[,]
                {
                    { 1L, "https://example.com/iphone15pro.jpg", "iPhone 15 Pro", 999.99m, 0, "Latest iPhone with Pro features and titanium design." },
                    { 2L, "https://example.com/nike-airmax.jpg", "Nike Air Max", 129.99m, 1, "Comfortable running shoes with Air Max technology." },
                    { 3L, "https://example.com/macbook-pro.jpg", "MacBook Pro 14\"", 1999.99m, 0, "Powerful laptop with M3 chip for professional work." },
                    { 4L, "https://example.com/levis-501.jpg", "Levi's 501 Jeans", 79.99m, 1, "Classic straight-fit jeans in premium denim." },
                    { 5L, "https://example.com/samsung-tv.jpg", "Samsung 4K TV", 599.99m, 0, "55-inch 4K Smart TV with HDR and streaming apps." }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { 1L, "admin@shop.com", "admin123", 1, "admin" },
                    { 2L, "john.doe@example.com", "customer123", 0, "john_doe" },
                    { 3L, "jane.smith@example.com", "customer123", 0, "jane_smith" }
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

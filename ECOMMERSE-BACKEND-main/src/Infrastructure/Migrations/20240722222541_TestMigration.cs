using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TestMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(400)", nullable: false),
                    Stock = table.Column<int>(type: "int(4.0)", nullable: false),
                    ProductAvaible = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    RegisterDate = table.Column<DateOnly>(type: "datetime", nullable: false),
                    UserType = table.Column<int>(type: "INTEGER", nullable: false),
                    Avaible = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PaymentMethod = table.Column<int>(type: "INTEGER", nullable: false),
                    StatusOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false),
                    SellerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Users_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_Users_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderNotifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(400)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderNotifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderNotifications_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderNotifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrdersProducts",
                columns: table => new
                {
                    OrderProductId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdersProducts", x => x.OrderProductId);
                    table.ForeignKey(
                        name: "FK_OrdersProducts_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrdersProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "Name", "Price", "ProductAvaible", "Stock" },
                values: new object[,]
                {
                    { 1, "Talle L color blanco", "Remera", 100.00m, true, 500 },
                    { 2, "Talle 44 jean", "Pantalon", 200.00m, true, 300 },
                    { 3, "Talle 40 de color negro", "Zapatos", 300.00m, true, 400 },
                    { 4, "Talle 38 de color blanco", "Zapatos", 400.00m, true, 1500 },
                    { 5, "Talle 36 de color marron", "Zapatos", 350.00m, true, 1000 },
                    { 6, "Talle XL de color azul", "Buzo", 500.00m, true, 900 },
                    { 7, "Talle L de color blanco", "Buzo", 500.00m, true, 750 },
                    { 8, "Talle 40 marca Nike", "Zapatillas", 350.00m, true, 1200 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Avaible", "Email", "LastName", "Name", "Password", "RegisterDate", "UserType" },
                values: new object[,]
                {
                    { 1, true, "gaston.koch@hotmail.com", "Koch", "Gaston", "g", new DateOnly(2024, 6, 6), 1 },
                    { 2, true, "distefanoalejandrom@gmail.com", "Di Stefano", "Alejandro", "2", new DateOnly(2024, 4, 5), 0 },
                    { 3, true, "juan@gmail.com", "Gomez", "Juan", "3", new DateOnly(2024, 2, 1), 1 },
                    { 4, true, "ana@gmail.com", "Lopez", "Ana", "4", new DateOnly(2024, 5, 10), 0 },
                    { 5, true, "luis@gmail.com", "Franco", "Luis", "123", new DateOnly(2024, 3, 15), 1 },
                    { 6, true, "pepe@gmail.com", "Moscheti", "Pepe", "876", new DateOnly(2024, 3, 15), 1 },
                    { 7, true, "admin@ecommerce.com", "", "admin", "admin", new DateOnly(2024, 3, 15), 2 },
                    { 8, true, "luisG@hotmail.com", "Guevara", "Luis", "433", new DateOnly(2024, 3, 15), 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderNotifications_OrderId",
                table: "OrderNotifications",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderNotifications_UserId",
                table: "OrderNotifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_SellerId",
                table: "Orders",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersProducts_OrderId",
                table: "OrdersProducts",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersProducts_ProductId",
                table: "OrdersProducts",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderNotifications");

            migrationBuilder.DropTable(
                name: "OrdersProducts");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

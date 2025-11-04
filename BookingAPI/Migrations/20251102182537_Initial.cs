using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingAPI.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PricePerNight = table.Column<int>(type: "int", nullable: false),
                    MaxGuests = table.Column<int>(type: "int", nullable: false),
                    Bedrooms = table.Column<int>(type: "int", nullable: false),
                    Bathrooms = table.Column<int>(type: "int", nullable: false),
                    Sqm = table.Column<int>(type: "int", nullable: false),
                    HasWifi = table.Column<bool>(type: "bit", nullable: false),
                    HasAirConditioning = table.Column<bool>(type: "bit", nullable: false),
                    HasKitchen = table.Column<bool>(type: "bit", nullable: false),
                    HasWashingMachine = table.Column<bool>(type: "bit", nullable: false),
                    HasParking = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PropertyId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalPrice = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Properties",
                columns: new[] { "Id", "Address", "Bathrooms", "Bedrooms", "City", "Country", "Description", "HasAirConditioning", "HasKitchen", "HasParking", "HasWashingMachine", "HasWifi", "ImageUrl", "MaxGuests", "PricePerNight", "Sqm", "Title" },
                values: new object[,]
                {
                    { 1, "Moo 2, Chaweng Beach", 1, 1, "Koh Samui", "Thailand", "Cozy wooden bungalow just 50 meters from the turquoise waters of Koh Samui. Ideal for couples looking for a peaceful escape.", true, false, true, false, true, "https://images.unsplash.com/photo-1717119125410-29c3edf6e075?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&q=80&w=1170", 2, 85, 35, "Seaside Bungalow" },
                    { 2, "123 Sunset Road", 2, 3, "Phuket", "Thailand", "Spacious luxury villa on Phuket's west coast with private pool and panoramic ocean view. Perfect for families or groups.", true, true, true, true, true, "https://plus.unsplash.com/premium_photo-1682377521697-bc598b52b08a?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&q=80&w=1215", 6, 290, 180, "Luxury Villa with Ocean View" },
                    { 3, "Haad Yuan Bay", 1, 1, "Koh Phangan", "Thailand", "Rustic bamboo hut located right on the beach of Koh Phangan. Simple but magical experience for nature lovers.", false, false, false, false, false, "https://plus.unsplash.com/premium_photo-1687960116228-13d383d20188?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&q=80&w=1170", 2, 50, 28, "Beachfront Bamboo Hut" },
                    { 4, "89/7 Beach Avenue", 1, 1, "Phuket", "Thailand", "Modern and stylish condo just 5 minutes walk from Patong Beach. Includes pool access and gym.", true, true, true, false, true, "https://plus.unsplash.com/premium_photo-1687960117069-567a456fe5f3?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&q=80&w=1170", 3, 110, 60, "Modern Condo near Patong Beach" },
                    { 5, "Soi Ao Nang 6", 2, 2, "Krabi", "Thailand", "Charming villa surrounded by lush tropical gardens in Krabi. 10-minute walk to Ao Nang beach. Great for families with kids.", true, true, true, true, true, "https://plus.unsplash.com/premium_photo-1675745329378-5573c360f69f?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&q=80&w=1170", 4, 160, 95, "Tropical Garden Villa" },
                    { 6, "Ocean Cliff Road", 2, 2, "Koh Tao", "Thailand", "Exclusive villa perched on a cliff in Koh Tao with private infinity pool and breathtaking sunset views.", true, true, true, true, true, "https://images.unsplash.com/photo-1728050829093-9ee62013968a?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&q=80&w=1171", 5, 320, 140, "Cliffside Pool Villa" },
                    { 7, "Beachside Lane 5", 1, 1, "Hua Hin", "Thailand", "Bright minimalist-style studio located in Hua Hin, 200 meters from the beach. Great for remote work and relaxation.", true, true, true, false, true, "https://images.unsplash.com/photo-1543489822-c49534f3271f?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&q=80&w=1332", 2, 70, 40, "Minimalist Studio near the Sea" },
                    { 8, "Ban Pak Meng", 1, 2, "Trang", "Thailand", "Authentic Thai teakwood house in Chiang Mai style, relocated to a quiet seaside village near Trang. A cultural and peaceful stay.", false, true, true, false, true, "https://images.unsplash.com/photo-1692736933760-8a8a9b8c1b6f?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&q=80&w=1074", 4, 95, 85, "Traditional Thai House" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_PropertyId",
                table: "Bookings",
                column: "PropertyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Properties");
        }
    }
}

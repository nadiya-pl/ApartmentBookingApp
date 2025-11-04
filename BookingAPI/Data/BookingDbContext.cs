using BookingAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookingAPI.Data;

public class BookingDbContext: DbContext
{
    public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options) { }

    public DbSet<Property> Properties { get; set; }
    public DbSet<Booking> Bookings { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Property>()
            .HasData
            (
                new Property
                {
                    Id = 1,
                    Title = "Seaside Bungalow",
                    Description = "Cozy wooden bungalow just 50 meters from the turquoise waters of Koh Samui. Ideal for couples looking for a peaceful escape.",
                    Address = "Moo 2, Chaweng Beach",
                    City = "Koh Samui",
                    Country = "Thailand",
                    ImageUrl = "https://images.unsplash.com/photo-1717119125410-29c3edf6e075?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&q=80&w=1170",
                    PricePerNight = 85,
                    MaxGuests = 2,
                    Bedrooms = 1,
                    Bathrooms = 1,
                    Sqm = 35,
                    HasWifi = true,
                    HasAirConditioning = true,
                    HasKitchen = false,
                    HasWashingMachine = false,
                    HasParking = true
                },
                new Property
                {
                    Id = 2,
                    Title = "Luxury Villa",
                    Description = "Spacious luxury villa on Phuket's west coast with private pool and panoramic ocean view. Perfect for families or groups.",
                    Address = "123 Sunset Road",
                    City = "Phuket",
                    Country = "Thailand",
                    ImageUrl = "https://plus.unsplash.com/premium_photo-1682377521697-bc598b52b08a?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&q=80&w=1215",
                    PricePerNight = 290,
                    MaxGuests = 6,
                    Bedrooms = 3,
                    Bathrooms = 2,
                    Sqm = 180,
                    HasWifi = true,
                    HasAirConditioning = true,
                    HasKitchen = true,
                    HasWashingMachine = true,
                    HasParking = true
                },
                new Property
                {
                    Id = 3,
                    Title = "Beachfront Bamboo Hut",
                    Description = "Rustic bamboo hut located right on the beach of Koh Phangan. Simple but magical experience for nature lovers.",
                    Address = "Haad Yuan Bay",
                    City = "Koh Phangan",
                    Country = "Thailand",
                    ImageUrl = "https://plus.unsplash.com/premium_photo-1687960116228-13d383d20188?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&q=80&w=1170",
                    PricePerNight = 50,
                    MaxGuests = 2,
                    Bedrooms = 1,
                    Bathrooms = 1,
                    Sqm = 28,
                    HasWifi = false,
                    HasAirConditioning = false,
                    HasKitchen = false,
                    HasWashingMachine = false,
                    HasParking = false
                },
                new Property
                {
                    Id = 4,
                    Title = "Modern Condo",
                    Description = "Modern and stylish condo just 5 minutes walk from Patong Beach. Includes pool access and gym.",
                    Address = "89/7 Beach Avenue",
                    City = "Phuket",
                    Country = "Thailand",
                    ImageUrl = "https://plus.unsplash.com/premium_photo-1687960117069-567a456fe5f3?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&q=80&w=1170",
                    PricePerNight = 110,
                    MaxGuests = 3,
                    Bedrooms = 1,
                    Bathrooms = 1,
                    Sqm = 60,
                    HasWifi = true,
                    HasAirConditioning = true,
                    HasKitchen = true,
                    HasWashingMachine = false,
                    HasParking = true
                },
                new Property
                {
                    Id = 5,
                    Title = "Tropical Garden Villa",
                    Description = "Charming villa surrounded by lush tropical gardens in Krabi. 10-minute walk to Ao Nang beach. Great for families with kids.",
                    Address = "Soi Ao Nang 6",
                    City = "Krabi",
                    Country = "Thailand",
                    ImageUrl = "https://plus.unsplash.com/premium_photo-1675745329378-5573c360f69f?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&q=80&w=1170",
                    PricePerNight = 160,
                    MaxGuests = 4,
                    Bedrooms = 2,
                    Bathrooms = 2,
                    Sqm = 95,
                    HasWifi = true,
                    HasAirConditioning = true,
                    HasKitchen = true,
                    HasWashingMachine = true,
                    HasParking = true
                },
                new Property
                {
                    Id = 6,
                    Title = "Cliffside Pool Villa",
                    Description = "Exclusive villa perched on a cliff in Koh Tao with private infinity pool and breathtaking sunset views.",
                    Address = "Ocean Cliff Road",
                    City = "Koh Tao",
                    Country = "Thailand",
                    ImageUrl = "https://images.unsplash.com/photo-1728050829093-9ee62013968a?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&q=80&w=1171",
                    PricePerNight = 320,
                    MaxGuests = 5,
                    Bedrooms = 2,
                    Bathrooms = 2,
                    Sqm = 140,
                    HasWifi = true,
                    HasAirConditioning = true,
                    HasKitchen = true,
                    HasWashingMachine = true,
                    HasParking = true
                },
                new Property
                {
                    Id = 7,
                    Title = "Minimalist Studio",
                    Description = "Bright minimalist-style studio located in Hua Hin, 200 meters from the beach. Great for remote work and relaxation.",
                    Address = "Beachside Lane 5",
                    City = "Hua Hin",
                    Country = "Thailand",
                    ImageUrl = "https://images.unsplash.com/photo-1543489822-c49534f3271f?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&q=80&w=1332",
                    PricePerNight = 70,
                    MaxGuests = 2,
                    Bedrooms = 1,
                    Bathrooms = 1,
                    Sqm = 40,
                    HasWifi = true,
                    HasAirConditioning = true,
                    HasKitchen = true,
                    HasWashingMachine = false,
                    HasParking = true
                },
                new Property
                {
                    Id = 8,
                    Title = "Traditional Thai House",
                    Description = "Authentic Thai teakwood house in Chiang Mai style, relocated to a quiet seaside village near Trang. A cultural and peaceful stay.",
                    Address = "Ban Pak Meng",
                    City = "Trang",
                    Country = "Thailand",
                    ImageUrl = "https://images.unsplash.com/photo-1692736933760-8a8a9b8c1b6f?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&q=80&w=1074",
                    PricePerNight = 95,
                    MaxGuests = 4,
                    Bedrooms = 2,
                    Bathrooms = 1,
                    Sqm = 85,
                    HasWifi = true,
                    HasAirConditioning = false,
                    HasKitchen = true,
                    HasWashingMachine = false,
                    HasParking = true
                }
            );
    }
}

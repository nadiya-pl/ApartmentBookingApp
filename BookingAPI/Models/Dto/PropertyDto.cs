namespace BookingAPI.Models.Dto;

public class PropertyDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;

    public int PricePerNight { get; set; }
    public int MaxGuests { get; set; }
    public int Bedrooms { get; set; }
    public int Bathrooms { get; set; }
    public int Sqm { get; set; }

    public bool HasWifi { get; set; }
    public bool HasAirConditioning { get; set; }
    public bool HasKitchen { get; set; }
    public bool HasWashingMachine { get; set; }
    public bool HasParking { get; set; }
}

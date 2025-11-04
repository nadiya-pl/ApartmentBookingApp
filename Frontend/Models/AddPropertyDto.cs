using System.ComponentModel.DataAnnotations;

namespace Frontend.Models;
public class AddPropertyDto
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, ErrorMessage = "Title must be less than 100 characters")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required")]
    [StringLength(1000, ErrorMessage = "Description must be less than 1000 characters")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Address is required")]
    public string Address { get; set; } = string.Empty;

    [Required(ErrorMessage = "City is required")]
    public string City { get; set; } = string.Empty;

    [Required(ErrorMessage = "Country is required")]
    public string Country { get; set; } = string.Empty;

    [Required(ErrorMessage = "Image URL is required")]
    [Url(ErrorMessage = "Invalid image URL format")]
    public string ImageUrl { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Price per night must be greater than 0")]
    public int PricePerNight { get; set; }

    [Range(1, 100, ErrorMessage = "Max guests must be between 1 and 100")]
    public int MaxGuests { get; set; }

    [Range(0, 50, ErrorMessage = "Bedrooms must be between 0 and 50")]
    public int Bedrooms { get; set; }

    [Range(0, 50, ErrorMessage = "Bathrooms must be between 0 and 50")]
    public int Bathrooms { get; set; }

    [Range(1, 10000, ErrorMessage = "Square meters must be greater than 0")]
    public int Sqm { get; set; }

    public bool HasWifi { get; set; }
    public bool HasAirConditioning { get; set; }
    public bool HasKitchen { get; set; }
    public bool HasWashingMachine { get; set; }
    public bool HasParking { get; set; }
}

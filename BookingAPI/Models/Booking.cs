using Microsoft.AspNetCore.Identity;

namespace BookingAPI.Models;

public class Booking
{
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;
    public int PropertyId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int TotalPrice { get; set; }
    public Property Property { get; set; } = null!;
}


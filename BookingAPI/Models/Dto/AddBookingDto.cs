namespace BookingAPI.Models.Dto;

public class AddBookingDto
{
    public string UserId { get; set; } = string.Empty;
    public int PropertyId { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public int TotalPrice { get; set; }
}

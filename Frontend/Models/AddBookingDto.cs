using System.ComponentModel.DataAnnotations;

namespace Frontend.Models;

public class AddBookingDto
{
    public string UserId { get; set; } = string.Empty;
    public int PropertyId { get; set; }

    [Required(ErrorMessage = "Start date is required")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "End date is required")]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }

    public int TotalPrice { get; set; }
}

using BookingAPI.Models;
using BookingAPI.Models.Dto;

namespace BookingAPI.Services;

public interface IBookingService
{
    Task<List<BookingDto>> GetAllBookingsAsync(CancellationToken ct);
    Task<Booking?> GetBookingByIdAsync(int id, CancellationToken ct);
    Task<Booking> AddNewBookingAsync(Booking newBooking, CancellationToken ct);
    Task<List<Booking>> GetBookingsByUserAsync(string userId, CancellationToken ct);
    Task<List<Booking>> GetBookingsForPropertyAsync(int propertyId, CancellationToken ct);
}

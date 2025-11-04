using BookingAPI.Data;
using BookingAPI.HttpClients;
using BookingAPI.Models;
using BookingAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace BookingAPI.Services;

public class BookingService : IBookingService
{
    private readonly BookingDbContext _db;
    private readonly AuthHttpClient _client;
    private readonly ILogger<BookingService> _logger;   

    public BookingService(BookingDbContext db, AuthHttpClient client, ILogger<BookingService> logger)
    {
        _db = db;
        _client = client;
        _logger = logger;
    }

    public async Task<List<BookingDto>> GetAllBookingsAsync(CancellationToken ct)
    {
        var bookings = await _db.Bookings.Include(p => p.Property).AsNoTracking().ToListAsync(ct);

        if (bookings.Count == 0)
            return new List<BookingDto>();

        var userIds = bookings.Select(b => b.UserId).Distinct().ToList();

        var users = await _client.GetSelectedUsers(userIds);

        var userDict = users?.Data != null
           ? users.Data.ToDictionary(u => u.Id)
           : new Dictionary<string, UserInfoDto>();


        var result = bookings.Select(b => new BookingDto
        {
            Id = b.Id,
            PropertyId = b.PropertyId,
            PropertyTitle = b.Property.Title,
            UserId = b.UserId,
            StartDate = b.StartDate,
            EndDate = b.EndDate,
            CreatedAt = b.CreatedAt,
            TotalPrice = b.TotalPrice,
            FirstName = userDict.TryGetValue(b.UserId, out var user) ? user.FirstName : "",
            LastName = userDict.TryGetValue(b.UserId, out var user2) ? user2.LastName : ""
        }).ToList();

        return result;
    }


    public async Task<Booking> AddNewBookingAsync(Booking newBooking, CancellationToken ct)
    {
        await _db.AddAsync(newBooking, ct);
        await _db.SaveChangesAsync(ct);

        _logger.LogInformation("Booking created successfully with ID {Id}.", newBooking.Id);

        return newBooking;
    }
    

    public async Task<Booking?> GetBookingByIdAsync(int id, CancellationToken ct)
    {
        return await _db.Bookings.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<List<Booking>> GetBookingsByUserAsync(string userId, CancellationToken ct)
    {
        return await _db.Bookings.AsNoTracking().Where(x => x.UserId == userId).ToListAsync(ct);
    }

    public async Task<List<Booking>> GetBookingsForPropertyAsync(int propertyId, CancellationToken ct)
    {
        return await _db.Bookings.AsNoTracking().Where(x => x.PropertyId == propertyId).ToListAsync(ct);
    }
}

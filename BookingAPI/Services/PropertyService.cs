using BookingAPI.Data;
using BookingAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingAPI.Services;

public class PropertyService : IPropertyService
{
    private readonly BookingDbContext _db;
    private readonly ILogger<PropertyService> _logger;

    public PropertyService(BookingDbContext db, ILogger<PropertyService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<List<Property>> GetPropertiesAsync(CancellationToken ct)
    {
        return await _db.Properties.AsNoTracking().ToListAsync(ct);
    }

    public async Task<Property?> GetPropertyByIdAsync(int id, CancellationToken ct)
    {
        var property = await _db.Properties.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, ct);
        return property;
    }

    public async Task<Property> AddPropertyAsync(Property newProperty, CancellationToken ct)
    {
        await _db.Properties.AddAsync(newProperty, ct);
        await _db.SaveChangesAsync(ct);

        _logger.LogInformation("Added new property with ID {Id}", newProperty.Id);

        return newProperty;
    }

    public async Task<bool> DeletePropertyAsync(int id, CancellationToken ct)
    {
        var property = await _db.Properties.FirstOrDefaultAsync(p => p.Id == id, ct);

        if (property == null)
        {
            _logger.LogWarning("Attempted to delete property with ID {Id}, but it was not found.", id);
            return false;
        }

        _db.Remove(property);
        await _db.SaveChangesAsync(ct);

        _logger.LogInformation("Deleted property with ID {Id}", id);

        return true;
    }
}

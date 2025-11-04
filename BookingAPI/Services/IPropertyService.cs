using BookingAPI.Models;

namespace BookingAPI.Services;

public interface IPropertyService
{
    Task<List<Property>> GetPropertiesAsync(CancellationToken ct);
    Task<Property?> GetPropertyByIdAsync(int id, CancellationToken ct);
    Task<Property> AddPropertyAsync(Property newProperty, CancellationToken ct);
    Task<bool> DeletePropertyAsync(int id, CancellationToken ct);
}

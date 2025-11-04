using BookingAPI.Mappers;

namespace BookingAPI.Extensions;

public static class MappingExtensions
{
    public static IServiceCollection AddAppMapping(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingConfig));
        return services;
    }
}

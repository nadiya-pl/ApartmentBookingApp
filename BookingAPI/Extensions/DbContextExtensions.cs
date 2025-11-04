using BookingAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace BookingAPI.Extensions;

public static class DbContextExtensions
{
    public static IServiceCollection AddAppDbContext(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<BookingDbContext>(options =>
        {
            options.UseSqlServer(config.GetConnectionString("BookingConnectionString"));
        });

        return services;
    }
}

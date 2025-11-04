using Serilog;
using Serilog.Events;

namespace BookingAPI.Extensions;

public static class SerilogExtensions
{
    public static void AddSerilogLogging(this IHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog((context, services, configuration) =>
        {
            configuration
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithThreadId()
                .Enrich.WithEnvironmentName()
                .WriteTo.Console()
                .WriteTo.File(
                    path: "Logs/log-.txt",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7,
                    restrictedToMinimumLevel: LogEventLevel.Information,
                    outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] ({ThreadId}) {Message:lj}{NewLine}{Exception}"
                );
        });
    }
}
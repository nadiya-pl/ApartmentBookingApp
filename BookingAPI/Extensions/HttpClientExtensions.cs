using BookingAPI.HttpClients;
using BookingAPI.Policies;

namespace BookingAPI.Extensions;

public static class HttpClientExtensions
{
    public static IServiceCollection AddAppHttpClients(this IServiceCollection services, IConfiguration config)
    {
        var authServiceUrl = config.GetValue<string>("Services:Auth");
        services.AddHttpClient<AuthHttpClient>(client =>
        {
            client.BaseAddress = new Uri(authServiceUrl!);
        })
            .AddPolicyHandler(PollyPolicies.TimeoutPolicy);

        return services;
    }
}

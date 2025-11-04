using Frontend.HttpClients;
using Frontend.Policies;

namespace Frontend.Extensions;

public static class HttpClientExtensions
{
    public static void AddHttpClients(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;

        builder.Services.AddHttpClient<BookingHttpClient>(client =>
        {
            client.BaseAddress = new Uri(configuration.GetValue<string>("Services:Booking")!);
        })
            .AddPolicyHandler(PollyPolicies.TimeoutPolicy);

        builder.Services.AddHttpClient<AuthHttpClient>(client =>
        {
            client.BaseAddress = new Uri(configuration.GetValue<string>("Services:Auth")!);
        })
            .AddPolicyHandler(PollyPolicies.TimeoutPolicy);
    }
}

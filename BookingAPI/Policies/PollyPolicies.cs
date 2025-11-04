using Polly;
using Serilog;

namespace BookingAPI.Policies;

public static class PollyPolicies
{
    public static IAsyncPolicy<HttpResponseMessage> TimeoutPolicy =>
        Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(8));
}
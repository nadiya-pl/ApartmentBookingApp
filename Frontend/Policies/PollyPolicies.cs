using Polly;
using Serilog;

namespace Frontend.Policies;

public static class PollyPolicies
{
    public static IAsyncPolicy<HttpResponseMessage> TimeoutPolicy =>
        Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(10));
}

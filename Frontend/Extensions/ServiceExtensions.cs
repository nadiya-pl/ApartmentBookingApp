using Frontend.Filters;
using Frontend.Token;
using Microsoft.AspNetCore.Authentication.Cookies;
using static System.Net.WebRequestMethods;

namespace Frontend.Extensions;

public static class ServiceExtensions
{
    public static void AddApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllersWithViews(options =>
        {
            options.Filters.Add<AuthRedirectExceptionFilter>();
        });

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<ITokenManager, TokenManager>();
    }

    public static void AddAuthenticationAndSession(this WebApplicationBuilder builder)
    {
        // Session & cache
        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        // Cookie authentication
        builder.Services
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromHours(10);
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

                options.Events.OnRedirectToLogin = context =>
                {
                    if (context.Response.StatusCode == StatusCodes.Status401Unauthorized ||
                        context.Response.StatusCode == StatusCodes.Status403Forbidden)
                    {
                        context.Response.Redirect($"/Account/Login");
                    }

                    return Task.CompletedTask;
                };
            });
    }
}
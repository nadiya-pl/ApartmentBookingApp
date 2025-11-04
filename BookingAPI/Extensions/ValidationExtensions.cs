using BookingAPI.Validators;
using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BookingAPI.Extensions;

public static class ValidationExtensions
{
    public static IServiceCollection AddAppValidation(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<AddPropertyValidator>();

        return services;
    }
}

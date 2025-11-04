using BookingAPI.Models;
using BookingAPI.Models.Dto;
using FluentValidation;

namespace BookingAPI.Validators;

public class AddPropertyValidator: AbstractValidator<AddPropertyDto>
{
    public AddPropertyValidator()
    {
        CascadeMode = CascadeMode.Stop;

        RuleFor(p => p.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

        RuleFor(p => p.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

        RuleFor(p => p.Address)
            .NotEmpty().WithMessage("Address is required.")
            .MaximumLength(200).WithMessage("Address must not exceed 200 characters.");

        RuleFor(p => p.City)
            .NotEmpty().WithMessage("City is required.")
            .MaximumLength(100).WithMessage("City must not exceed 100 characters.");

        RuleFor(p => p.Country)
            .NotEmpty().WithMessage("Country is required.")
            .MaximumLength(100).WithMessage("Country must not exceed 100 characters.");

    
        RuleFor(p => p.ImageUrl)
            .NotEmpty().WithMessage("Image URL is required.");

      
        RuleFor(p => p.PricePerNight)
            .GreaterThan(0).WithMessage("Price per night must be greater than 0.")
            .LessThanOrEqualTo(10000).WithMessage("Price per night seems too high.");

        RuleFor(p => p.MaxGuests)
            .InclusiveBetween(1, 20).WithMessage("Guests count must be between 1 and 20.");

        RuleFor(p => p.Bedrooms)
            .InclusiveBetween(0, 10).WithMessage("Bedrooms count must be between 0 and 10.");

        RuleFor(p => p.Bathrooms)
            .InclusiveBetween(0, 10).WithMessage("Bathrooms count must be between 0 and 10.");

        RuleFor(p => p.Sqm)
            .GreaterThan(0).WithMessage("Property size must be greater than 0.")
            .LessThanOrEqualTo(1000).WithMessage("Property size must not exceed 1000 m².");
    }
}

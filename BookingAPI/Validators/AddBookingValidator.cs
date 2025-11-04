using BookingAPI.Models.Dto;
using FluentValidation;

namespace BookingAPI.Validators;

public class AddBookingValidator: AbstractValidator<AddBookingDto>
{
    public AddBookingValidator()
    {
        CascadeMode = CascadeMode.Stop;

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.");

        RuleFor(x => x.PropertyId)
            .GreaterThan(0).WithMessage("Property ID must be greater than 0.");

        RuleFor(x => x.StartDate)
            .Must(CheckValidDate).WithMessage("Start date is required.")
            .LessThan(x => x.EndDate).WithMessage("Start date must be before end date.");

        RuleFor(x => x.EndDate)
            .Must(CheckValidDate).WithMessage("End date is required.")
            .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date.");
    }

    private bool CheckValidDate(DateTime date)
    {
        return date != default;
    }
}

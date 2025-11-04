using AutoMapper;
using BookingAPI.Models;
using BookingAPI.Models.Dto;
using BookingAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.IdentityModel.Tokens;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookingAPI.Controllers;


[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly ILogger<BookingController> _logger;
    private readonly IBookingService _service;
    private readonly IPropertyService _propertyService;
    private readonly IMapper _mapper;

    public BookingController(IBookingService service, IPropertyService propertyService, IMapper mapper, ILogger<BookingController> logger)
    {
        _logger = logger;
        _mapper = mapper;
        _service = service;
        _propertyService = propertyService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllBookings(CancellationToken ct)
    {
        var response = new ApiResponseDto();

        var bookings = await _service.GetAllBookingsAsync(ct);

        response.Success = true;
        response.Data = bookings;

        return Ok(response);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetBookingById(int id, CancellationToken ct)
    {
        var response = new ApiResponseDto();

        if (id <= 0)
        {
            response.Success = false;
            response.Message = "Please provide a valid booking ID.";

            _logger.LogWarning("Invalid booking ID: {Id}", id);

            return BadRequest(response);
        }

        var booking = await _service.GetBookingByIdAsync(id, ct);

        if (booking == null)
        {
            response.Success = false;
            response.Message = "Booking not found.";

            _logger.LogWarning("Booking not found for ID {Id}", id);

            return NotFound(response);
        }

        response.Success = true;
        response.Data = _mapper.Map<BookingDto>(booking);
        return Ok(response);
    }

    [HttpGet("user/{id}")]
    [Authorize]
    public async Task<IActionResult> GetBookingsByUser(string id, CancellationToken ct)
    {
        var response = new ApiResponseDto();

        if (string.IsNullOrWhiteSpace(id))
        {
            response.Success = false;
            response.Message = "Please provide a valid user ID.";

            _logger.LogWarning("Invalid user ID provided for booking lookup.");

            return BadRequest(response);
        }

        var bookings = await _service.GetBookingsByUserAsync(id, ct);

        if (!bookings.Any())
        {
            response.Success = false;
            response.Message = "No bookings found for this user.";

            _logger.LogInformation("No bookings found for user {UserId}", id);

            return NotFound(response);
        }

        response.Success = true;
        response.Data = _mapper.Map<IEnumerable<BookingDto>>(bookings);
        return Ok(response);
    }

    [HttpGet("property/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetBookingsForProperty(int id, CancellationToken ct)
    {
        var response = new ApiResponseDto();

        if (id <= 0)
        {
            response.Success = false;
            response.Message = "Please provide a valid property ID.";

            _logger.LogWarning("Invalid property ID: {Id}", id);

            return BadRequest(response);
        }

        var bookings = await _service.GetBookingsForPropertyAsync(id, ct);

        if (!bookings.Any())
        {
            response.Success = false;
            response.Message = "No bookings found for this property.";

            _logger.LogInformation("No bookings found for property {Id}", id);

            return NotFound(response);
        }

        response.Success = true;
        response.Data = _mapper.Map<IEnumerable<BookingDto>>(bookings);
        return Ok(response);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddNewBooking([FromBody] AddBookingDto newBooking, CancellationToken ct)
    {
        var response = new ApiResponseDto();

        if (!ModelState.IsValid)
        {
            response.Success = false;
            response.Message = "Validation error.";
            response.Errors = GetValidationErrors(ModelState);

            _logger.LogWarning("Validation error when adding booking: {@Errors}", GetValidationErrors(ModelState));

            return BadRequest(response);
        }

        var property = await _propertyService.GetPropertyByIdAsync(newBooking.PropertyId, ct);
        if (property == null)
        {
            response.Success = false;
            response.Message = "Invalid Property ID.";

            _logger.LogWarning("Attempt to book non-existing property {PropertyId}", newBooking.PropertyId);

            return NotFound(response);
        }

        int days = (newBooking.EndDate.Date - newBooking.StartDate.Date).Days;
        newBooking.TotalPrice = property.PricePerNight * days;

        _logger.LogInformation("Creating booking for property {PropertyId}, {Days} days, total {TotalPrice}$.",
           property.Id, days, newBooking.TotalPrice);

        var booking = await _service.AddNewBookingAsync(_mapper.Map<Booking>(newBooking), ct);

        response.Success = true;
        response.Data = _mapper.Map<BookingDto>(booking);

        _logger.LogInformation("Booking created successfully with ID {Id}", booking.Id);

        return Ok(response);
    }

    private Dictionary<string, string> GetValidationErrors(ModelStateDictionary modelState)
    {
        return modelState
            .Where(kvp => kvp.Value.Errors.Any())
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.First().ErrorMessage
            );
    }
}


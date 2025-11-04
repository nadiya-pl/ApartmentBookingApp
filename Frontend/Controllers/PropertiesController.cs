using Frontend.HttpClients;
using Frontend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers;

public class PropertiesController : Controller
{
    private readonly BookingHttpClient _client;
    private readonly ILogger<PropertiesController> _logger;

    public PropertiesController(BookingHttpClient client, ILogger<PropertiesController> logger)
    {
        _client = client;
        _logger = logger;
    }


    public async Task<IActionResult> Index()
    {
        var properties = await _client.GetAllProperties();

        if (!properties.Success || properties.Data == null)
        {
            _logger.LogWarning("Failed to load properties. Reason: {Message}", properties.Message);
            TempData["ErrorMessage"] = "Failed to load property list from the server.";
            return RedirectToAction("Error", "Home");
        }

        return View(properties.Data);
    }

    public async Task<IActionResult> Details(int id)
    {
        var item = await _client.GetProperty(id);

        if (!item.Success || item.Data == null)
        {
            _logger.LogWarning("Property not found or server error. Id={Id}, Message={Message}", id, item.Message);
            TempData["ErrorMessage"] = "Property not found or failed to load.";
            return RedirectToAction("Error", "Home");
        }

        return View(item.Data);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(AddBookingDto newBooking)
    {
        var booking = await _client.AddNewBooking(newBooking);

        if (booking.Success == false && booking.Errors != null)
        {
            foreach (var error in booking.Errors)
            {
                ModelState.AddModelError(error.Key, error.Value); 
            }

            _logger.LogInformation("Validation failed when creating booking for PropertyId={PropertyId}", newBooking.PropertyId);

            var property = await _client.GetProperty(newBooking.PropertyId); 
            return View("Details", property.Data);

        }
        else if (booking.Success == false)
        {
            _logger.LogWarning("Booking creation failed. Reason: {Message}", booking.Message);
            TempData["ErrorMessage"] = "Failed to create booking. Reason: " + booking.Message;
            return RedirectToAction("Error", "Home");
        }

        _logger.LogInformation("Booking created successfully. Id={Id}, PropertyId={PropertyId}", booking.Data.Id, newBooking.PropertyId);

        return RedirectToAction("Thanks", "Home", new { id = booking.Data.Id });
    }
}

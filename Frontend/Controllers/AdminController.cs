using Frontend.HttpClients;
using Frontend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly BookingHttpClient _client;
    private readonly ILogger<AdminController> _logger;

    public AdminController(BookingHttpClient client, ILogger<AdminController> logger)
    {
        _client = client;
        _logger = logger;
    }

   
    public async Task<IActionResult> Properties()
    {
        var properties = await _client.GetAllProperties();

        if (!properties.Success || properties.Data == null)
        {
            _logger.LogWarning("[AdminController] Failed to load property list. Message: {Message}", properties.Message);
            TempData["ErrorMessage"] = properties.Message ?? "Failed to load properties from the server.";
            return RedirectToAction("Error", "Home");
        }

        return View(properties.Data);
    }

    public IActionResult Create()
    {
        return View(new AddPropertyDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create(AddPropertyDto propertyDto)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Invalid form data.");
            return View(propertyDto);
        }

        _logger.LogInformation("[AdminController] Attempting to create new property: {Title}", propertyDto.Title);

        var response = await _client.AddNewProperty(propertyDto);


        if (!response.Success && response.Errors != null)
        {
            foreach (var error in response.Errors)
                ModelState.AddModelError(error.Key, error.Value);

            return View(propertyDto);
        }
        else if (!response.Success)
        {
            ModelState.AddModelError(string.Empty, response.Message);
            return View(propertyDto);
        }

        _logger.LogInformation("[AdminController] Property created successfully: {Title}", propertyDto.Title);
        return RedirectToAction("Properties");
    }


    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("[AdminController] Deleting property with ID={Id}", id);

        var result = await _client.DeleteProperty(id);

        if (!result.Success)
        {
            _logger.LogWarning("[AdminController] Property deletion failed. Message: {Message}", result.Message);
            TempData["ErrorMessage"] = result.Message ?? "Delete failed.";
            return RedirectToAction("Error", "Home");
        }

        _logger.LogInformation("[AdminController] Property deleted successfully. ID={Id}", id);
        return RedirectToAction("Properties");
    }

    public async Task<IActionResult> Bookings()
    {
        var bookings = await _client.GetAllBookings();

        if (!bookings.Success || bookings.Data == null)
        {
            _logger.LogWarning("[AdminController] Failed to load booking list. Message: {Message}", bookings.Message);
            TempData["ErrorMessage"] = bookings.Message ?? "Failed to load bookings from the server.";
            return RedirectToAction("Error", "Home");
        }

        _logger.LogInformation("[AdminController] Loaded {Count} bookings.", bookings.Data.Count);
        return View(bookings.Data);
    }
}

using Frontend.HttpClients;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly BookingHttpClient _client;

    public HomeController(ILogger<HomeController> logger, BookingHttpClient client)
    {
        _logger = logger;
        _client = client;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Thanks(int id)
    {
        var booking = await _client.GetBookingById(id);

        if (!booking.Success || booking.Data == null)
        {
            _logger.LogWarning("Failed to load booking for ID={Id}. Reason: {Message}", id, booking.Message);
            TempData["ErrorMessage"] = "Failed to load booking details from the server.";
            return RedirectToAction("Error");
        }

        return View(booking.Data);
    }
    
    public IActionResult Error()
    {
        return View();
    }
}

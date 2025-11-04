using AutoMapper;
using Azure;
using BookingAPI.Models;
using BookingAPI.Models.Dto;
using BookingAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookingAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertiesController : ControllerBase
{
    private readonly ILogger<PropertiesController> _logger;
    private readonly IPropertyService _service;
    private readonly IMapper _mapper;


    public PropertiesController(IPropertyService service, IMapper mapper, ILogger<PropertiesController> logger)
    {
        _service = service;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
    public async Task<IActionResult> GetAllProperties(CancellationToken ct)
    {
        ApiResponseDto response = new ApiResponseDto();

        var properties = await _service.GetPropertiesAsync(ct);

        response.Success = true;
        response.Data = _mapper.Map<IEnumerable<PropertyDto>>(properties);
       
        return Ok(response);
    }

    [HttpGet("{id}")]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
    public async Task<IActionResult> GetProperty(int id, CancellationToken ct)
    {
        ApiResponseDto response = new ApiResponseDto();

        if (id <= 0)
        {
            response.Success = false;
            response.Message = "Please provide a valid property Id.";
            return BadRequest(response);
        }

        var property = await _service.GetPropertyByIdAsync(id, ct);
          
        if (property == null)
        {
            response.Success = false;
            response.Message = "Property not found.";

            _logger.LogWarning("Property not found for ID {Id}", id);

            return NotFound(response);
        }

        response.Success = true;
        response.Data = _mapper.Map<PropertyDto>(property);       
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddNewProperty([FromBody] AddPropertyDto newProperty, CancellationToken ct)
    {
        ApiResponseDto response = new ApiResponseDto();

        if (!ModelState.IsValid)
        {
            response.Success = false;
            response.Message = "Validation error.";
            response.Errors = GetValidationErrors(ModelState);

            _logger.LogWarning("Validation error when adding property: {@Errors}", GetValidationErrors(ModelState));

            return BadRequest(response);
        }

        var property = await _service.AddPropertyAsync(_mapper.Map<Property>(newProperty), ct);

        response.Success = true;
        response.Data = _mapper.Map<PropertyDto>(property);

        _logger.LogInformation("New property added with ID {Id}", property.Id);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteProperty(int id, CancellationToken ct)
    {
        ApiResponseDto response = new ApiResponseDto();

        if (id <= 0)
        {
            response.Success = false;
            response.Message = "Invalid property ID.";

            _logger.LogWarning("Invalid property ID for deletion: {Id}", id);

            return BadRequest(response);
        }

        bool deleted = await _service.DeletePropertyAsync(id, ct);

        if (!deleted)
        {
            response.Success = false;
            response.Message = "Property not found.";

            _logger.LogWarning("Attempt to delete non-existing property with ID {Id}", id);

            return NotFound(response);
        }

        response.Success = true;
        response.Message = "Property deleted successfully.";

        _logger.LogInformation("Property deleted successfully with ID {Id}", id);

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

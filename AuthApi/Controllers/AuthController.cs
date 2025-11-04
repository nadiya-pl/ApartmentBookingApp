using AuthAPI.Models.Dto;
using AuthAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AuthAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;

    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        AuthResponseDto result = new AuthResponseDto();

        if (!ModelState.IsValid)
        {
            result.Success = false;
            result.Message = "Validation error.";
            result.Errors = GetValidationsErrors(ModelState);

            _logger.LogWarning("Invalid registration request for email: {Email}", dto.Email);

            return BadRequest(result);
        }

        result = await _authService.RegisterAsync(dto);

        if (!result.Success)
        {
            _logger.LogWarning("Registration failed for {Email}. Reason: {Message}", dto.Email, result.Message);

            return BadRequest(result);
        }

        _logger.LogInformation("User registered successfully: {Email}", dto.Email);

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        AuthResponseDto result = new AuthResponseDto();

        if (!ModelState.IsValid)
        {
            result.Success = false;
            result.Message = "Validation error.";
            result.Errors = GetValidationsErrors(ModelState);

            _logger.LogWarning("Invalid login request for email: {Email}", dto.Email);

            return BadRequest(result);
        }

        result = await _authService.LoginAsync(dto);

        if (!result.Success)
        {
            _logger.LogWarning("Login failed for {Email}. Message: {Message}", dto.Email, result.Message);

            return BadRequest(result);
        }

        _logger.LogInformation("User logged in successfully: {Email}", dto.Email);
        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] TokensDto dto)
    {
        AuthResponseDto result = new AuthResponseDto();

        if (!ModelState.IsValid)
        {
            result.Success = false;
            result.Message = "Token validation error.";
            result.Errors = GetValidationsErrors(ModelState);

            _logger.LogWarning("Invalid refresh token request.");

            return BadRequest(result);
        }

        result = await _authService.RefreshTokensAsync(dto);

        if (!result.Success)
        {
            _logger.LogWarning("Token refresh failed. Message: {Message}", result.Message);

            return BadRequest(result);
        }
                
        return Ok(result);
    }

    private Dictionary<string, string> GetValidationsErrors(ModelStateDictionary modelState)
    {
        return modelState
        .Where(kvp => kvp.Value.Errors.Any())
        .ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value.Errors.First().ErrorMessage
        );
    }
}

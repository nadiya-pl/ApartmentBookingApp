using Frontend.HttpClients;
using Frontend.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Frontend.Token;
using System.IdentityModel.Tokens.Jwt;

namespace Frontend.Controllers;

public class AccountController : Controller
{
    private readonly AuthHttpClient _client;
    private readonly ITokenManager _tokenProvider;
    private readonly ILogger<AccountController> _logger;

    public AccountController(AuthHttpClient client, ITokenManager tokenProvider, ILogger<AccountController> logger)
    {
        _client = client;
        _tokenProvider = tokenProvider;
        _logger = logger;
    }


    public IActionResult Login(string? returnUrl = null)
    {
        TempData["ReturnUrl"] = returnUrl;
        return View(new LoginDto());
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Invalid form data.");
            return View(dto);
        }

        _logger.LogInformation("[AccountController] Login attempt for {Email}", dto.Email);

        var response = await _client.Login(dto);


     
        if (!response.Success && response.Errors != null)
        {
            _logger.LogWarning("[AccountController] Login validation failed for {Email}. Errors: {Count}",
               dto.Email, response.Errors.Count);

            foreach (var error in response.Errors)
                ModelState.AddModelError(error.Key, error.Value);

            return View(dto);
        }    
        else if (!response.Success)
        {
            _logger.LogWarning("[AccountController] Login failed for {Email}. Message: {Message}",
                dto.Email, response.Message);
            ModelState.AddModelError(string.Empty, response.Message);
            return View(dto);
        }
        else if (string.IsNullOrEmpty(response.Data.Token) || string.IsNullOrEmpty(response.Data.RefreshToken))
        {
            _logger.LogError("[AccountController] Login succeeded but tokens are missing for {Email}", dto.Email);
            ModelState.AddModelError(string.Empty, "Unexpected server response. Please try again.");
            return View(dto);
        }


        await SignInAsync(response.Data.Token);
        _tokenProvider.SetAccessToken(response.Data.Token);
        _tokenProvider.SetRefreshToken(response.Data.RefreshToken);

        _logger.LogInformation("[AccountController] Login successful for {Email}", dto.Email);

        var returnUrl = TempData["ReturnUrl"] as string;

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Register()
    {
        return View(new RegisterDto());
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Invalid form data.");
            return View(dto);
        }

        _logger.LogInformation("[AccountController] Registration attempt for {Email}", dto.Email);

        var response = await _client.Register(dto);


        if (!response.Success && response.Errors != null)
        {
            foreach (var error in response.Errors)
                ModelState.AddModelError(error.Key, error.Value);

            _logger.LogWarning("[AccountController] Registration validation failed for {Email}. Errors: {Count}",
            dto.Email, response.Errors.Count);
            return View(dto);
        }
        else if (!response.Success)
        {
            _logger.LogWarning("[AccountController] Registration failed for {Email}. Message: {Message}",
            dto.Email, response.Message);
            ModelState.AddModelError(string.Empty, response.Message);
            return View(dto);
        }
        else if (string.IsNullOrEmpty(response.Data.Token) || string.IsNullOrEmpty(response.Data.RefreshToken))
        {
            _logger.LogError("[AccountController] Registration succeeded but tokens are missing for {Email}", dto.Email);
            ModelState.AddModelError(string.Empty, "Unexpected server response. Please try again.");
            return View(dto);
        }


        await SignInAsync(response.Data.Token);
        _tokenProvider.SetAccessToken(response.Data.Token);
        _tokenProvider.SetRefreshToken(response.Data.RefreshToken);

        _logger.LogInformation("[AccountController] Registration successful for {Email}", dto.Email);

        return RedirectToAction("Index", "Home");
    }


    public async Task<IActionResult> RefreshTokens()
    {
        var tokensDto = new TokensDto();
        tokensDto.Token = _tokenProvider.GetAccessToken();
        tokensDto.RefreshToken = _tokenProvider.GetRefreshToken();

        if (string.IsNullOrEmpty(tokensDto.Token) || string.IsNullOrEmpty(tokensDto.RefreshToken))
        {
            _logger.LogWarning("[AccountController] RefreshTokens called but tokens are missing");
            return RedirectToAction("Login");
        }

        var response = await _client.RefreshTokens(tokensDto);


        if (response.Success == true)
        {
            await SignInAsync(response.Data.Token);
            _tokenProvider.SetAccessToken(response.Data.Token);
            _tokenProvider.SetRefreshToken(response.Data.RefreshToken);
                        
            var returnUrl = HttpContext.Session.GetString("ReturnUrl") ?? "";
            if(!string.IsNullOrWhiteSpace(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }
       
        return RedirectToAction("Login");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        _tokenProvider.ClearTokens();

        _logger.LogInformation("[AccountController] User logged out successfully.");

        return RedirectToAction("Index", "Home");
    }

    public IActionResult AccessDenied()
    {
        _logger.LogWarning("[AccountController] Access denied for user {User}", User.Identity?.Name ?? "Unknown");
        return View();
    }



    public async Task SignInAsync(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            GetClaims(jwt, identity);

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[AccountController] Error during SignInAsync: {Message}", ex.Message);
        }
    }

    public void GetClaims(JwtSecurityToken jwt, ClaimsIdentity identity)
    {
        try
        {
            var userId = jwt.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;

            var userName = jwt.Claims.FirstOrDefault(c => c.Type == "unique_name")?.Value;

            var email = jwt.Claims.FirstOrDefault(c => c.Type == "email")?.Value;

            var firstName = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName || c.Type == "given_name")?.Value;

            var roles = jwt.Claims.Where(c => c.Type == ClaimTypes.Role || c.Type == "role").Select(c => c.Value);
            foreach (var role in roles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            if (!string.IsNullOrEmpty(userId))
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId));

            if (!string.IsNullOrEmpty(userName))
                identity.AddClaim(new Claim(ClaimTypes.Name, userName));

            if (!string.IsNullOrEmpty(email))
                identity.AddClaim(new Claim(ClaimTypes.Email, email));

            if (!string.IsNullOrEmpty(firstName))
                identity.AddClaim(new Claim(ClaimTypes.GivenName, firstName));
        }
        catch { }
    }
}

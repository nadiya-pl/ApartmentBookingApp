using AuthAPI.Models;
using AuthAPI.Models.Dto;
using AuthAPI.TokenService;
using Microsoft.AspNetCore.Identity;

namespace AuthAPI.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signinManager;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly ILogger<AuthService> _logger;

    private readonly int refreshTokenLifetimeDays = 0;

    public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signinManager, ITokenGenerator tokenGenerator, ILogger<AuthService> logger, IConfiguration configuration)
    {
        _tokenGenerator = tokenGenerator;
        _userManager = userManager;
        _signinManager = signinManager;
        _logger = logger;
        refreshTokenLifetimeDays = configuration.GetValue<int>("JWT:RefreshTokenLifetimeDays", 30);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var response = new AuthResponseDto();

        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
        {
            _logger.LogWarning("Login failed. Reason: User does not exist. Email: {Email}", dto.Email);

            response.Success = false;
            response.Message = "User does not exist.";
            return response;
        }

        var passwordValidation = await _signinManager.PasswordSignInAsync(user, dto.Password, false, true);
        if (passwordValidation.IsLockedOut)
        {
            _logger.LogWarning("Login failed. Reason: Account temporary locked. Email: {Email}", dto.Email);

            response.Success = false;
            response.Message = "Account temporary locked.";
            return response;
        }

        if (!passwordValidation.Succeeded)
        {
            _logger.LogWarning("Login failed. Reason: Password is not correct. Email: {Email}", dto.Email);

            response.Success = false;
            response.Message = "Password is not correct.";
            return response;
        }

        _logger.LogInformation("User logged in successfully. Email: {Email}", user.Email);


        var refreshToken =  _tokenGenerator.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(refreshTokenLifetimeDays);
        await _userManager.UpdateAsync(user);

       
        var tokensDto = new TokensDto();
        tokensDto.Token = await _tokenGenerator.GenerateTokenAsync(user);
        tokensDto.RefreshToken = refreshToken;
        response.Data = tokensDto;
        response.Success = true;

        return response;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        var response = new AuthResponseDto();

        var existing = await _userManager.FindByEmailAsync(dto.Email);
        if (existing != null)
        {
            response.Success = false;
            response.Message = "User already exists.";
            return response;
        }

        var user = new AppUser
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            UserName = dto.Email,
            Email = dto.Email
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            response.Message = string.Join("", result.Errors.Select(e => e.Description));
            response.Success = false;

            _logger.LogWarning("User registration failed. Email: {Email}. Errors: {Errors}", dto.Email, string.Join(" | ", result.Errors.Select(e => e.Description)));
        }
        else
        {
            await _userManager.AddToRoleAsync(user, "Guest");

            var refreshToken = _tokenGenerator.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(refreshTokenLifetimeDays);
            await _userManager.UpdateAsync(user);

            var tokensDto = new TokensDto();
            tokensDto.Token = await _tokenGenerator.GenerateTokenAsync(user);
            tokensDto.RefreshToken = refreshToken;
            response.Data = tokensDto;
            response.Success = true;

            _logger.LogInformation("User registered successfully. Email: {Email}", user.Email);
        }

        return response;
    }

    public async Task<AuthResponseDto> RefreshTokensAsync(TokensDto dto)
    {
        var response = new AuthResponseDto();

        var email = _tokenGenerator.GetUserEmail(dto.Token);
        if (string.IsNullOrEmpty(email))
        {
            response.Success = false;
            response.Message = "Invalid access token.";
            return response;
        }

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null || user.RefreshToken != dto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            response.Success = false;
            response.Message = "Invalid refresh token.";
            return response;
        }

        var newAccessToken = await _tokenGenerator.GenerateTokenAsync(user);
        var newRefreshToken = _tokenGenerator.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(refreshTokenLifetimeDays);
        await _userManager.UpdateAsync(user);

        _logger.LogInformation("Refresh token updated for user {Email}", user.Email);


        var tokensDto = new TokensDto();
        tokensDto.Token = newAccessToken;
        tokensDto.RefreshToken = newRefreshToken;
        response.Data = tokensDto;
        response.Success = true;

        return response;
    }
}

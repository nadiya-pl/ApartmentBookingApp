using AuthAPI.Models;

namespace AuthAPI.TokenService;

public interface ITokenGenerator
{
    Task<string> GenerateTokenAsync(AppUser user);
    string GenerateRefreshToken();
    string GetUserEmail(string token);
}

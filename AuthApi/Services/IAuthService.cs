using AuthAPI.Models.Dto;

namespace AuthAPI.Services;

public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginDto dto);
    Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
    Task<AuthResponseDto> RefreshTokensAsync(TokensDto dto);
}

using AuthAPI.Models.Dto;

namespace AuthAPI.Services;

public interface IUsersService
{
    Task<AuthResponseDto> GetSelectedUsersAsync(List<string> userIds, CancellationToken ct);
    Task<AuthResponseDto> GetUserAsync(string userId, CancellationToken ct);
}

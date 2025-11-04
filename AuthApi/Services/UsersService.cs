using AuthAPI.Models;
using AuthAPI.Models.Dto;
using AuthAPI.TokenService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI.Services;

public class UsersService : IUsersService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<UsersService> _logger;

    public UsersService(UserManager<AppUser> userManager, ILogger<UsersService> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<AuthResponseDto> GetSelectedUsersAsync(List<string> userIds, CancellationToken ct)
    {
        var response = new AuthResponseDto();

        var users = await _userManager.Users
        .Where(u => userIds.Contains(u.Id))
        .Select(u => new UserInfoDto
        {
            Id = u.Id,
            FirstName = u.FirstName,
            LastName = u.LastName
        })
        .ToListAsync(ct);

        response.Success = true;
        response.Data = users;

        return response;
    }

    public async Task<AuthResponseDto> GetUserAsync(string userId, CancellationToken ct)
    {
        var response = new AuthResponseDto();

        var user = await _userManager.Users
            .Where(u => u.Id == userId)
            .Select(u => new UserInfoDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName
            })
            .FirstOrDefaultAsync(ct);

        if (user == null)
        {
            response.Success = false;
            response.Message = $"User with ID '{userId}' not found.";
            return response;
        }

        response.Success = true;
        response.Data = user;
        return response;
    }
}

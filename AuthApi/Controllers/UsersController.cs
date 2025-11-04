using AuthAPI.Models.Dto;
using AuthAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUsersService _service;
    public UsersController(IUsersService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(string id, CancellationToken ct)
    {
        AuthResponseDto response = new AuthResponseDto();

        if (string.IsNullOrWhiteSpace(id))
        {
            response.Success = false;
            response.Message = "Please provide a valid user ID.";
            return BadRequest(response);
        }

        var user = await _service.GetUserAsync(id, ct);

        if(!user.Success)
            return BadRequest(user);

        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> GetUsers([FromBody] List<string> userIds, CancellationToken ct)
    {
        var users = await _service.GetSelectedUsersAsync(userIds, ct);

        if (!users.Success)
            return BadRequest(users);

        return Ok(users);
    }
}

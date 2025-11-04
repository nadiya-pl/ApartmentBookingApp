using BookingAPI.Models.Dto;
using System.Text;
using System.Text.Json;

namespace BookingAPI.HttpClients;

public class AuthHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AuthHttpClient> _logger;

    public AuthHttpClient(HttpClient httpClient, ILogger<AuthHttpClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<AuthResponseDto<List<UserInfoDto>>> GetSelectedUsers(List<string> userIds)
    {
        try
        {
            var json = JsonSerializer.Serialize(userIds);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/users", content);
            return await HandleResponse<List<UserInfoDto>>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[AuthHttpClient] Error: {Message}", ex.Message);
            return null;
        }
    }

    public async Task<AuthResponseDto<UserInfoDto>> GetUserById(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync("api/users/" + id);
            return await HandleResponse<UserInfoDto>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[AuthHttpClient] Error: {Message}", ex.Message);
            return null;
        }
    }

    private async Task<AuthResponseDto<T>> HandleResponse<T>(HttpResponseMessage response)
    {
        var result = new AuthResponseDto<T>();
        try
        {
            var temp = await response.Content.ReadFromJsonAsync<AuthResponseDto<JsonElement>>();

            if (temp == null)
            {
                result.Success = false;
                result.Message = "Deserialization error.";
                return result;
            }

            result.Success = temp.Success;
            result.Message = temp.Message;
            result.TraceId = temp.TraceId;
            result.Errors = temp.Errors;
            result.Data = temp.Success ? temp.Data.Deserialize<T>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) : default;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Message = $"Error processing response: {ex.Message}";
            _logger.LogError(ex, "Error processing response: {Message}", ex.Message);
        }

        return result;
    }
}

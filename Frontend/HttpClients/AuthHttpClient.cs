using Frontend.Models;
using System.Text;
using System.Text.Json;

namespace Frontend.HttpClients;

public class AuthHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AuthHttpClient> _logger;
    public AuthHttpClient(HttpClient httpClient, ILogger<AuthHttpClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<AuthResponseDto<TokensDto>> Register(RegisterDto dto)
    {
        try
        {
            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/auth/register", content);

            return await HandleResponse<TokensDto>(response);
        }
        catch (Exception ex)
        {
            return CreateErrorResponse<TokensDto>(ex, "Register");
        }
    }

    public async Task<AuthResponseDto<TokensDto>> Login(LoginDto dto)
    {
        try
        {
            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/auth/login", content);

            return await HandleResponse<TokensDto>(response);
        }
        catch (Exception ex)
        {
            return CreateErrorResponse<TokensDto>(ex, "Login");
        }
    }

    public async Task<AuthResponseDto<TokensDto>> RefreshTokens(TokensDto dto)
    {
        try
        {
            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/auth/refresh", content);

            return await HandleResponse<TokensDto>(response);
        }
        catch (Exception ex)
        {
            return CreateErrorResponse<TokensDto>(ex, "RefreshTokens");
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
            _logger.LogError(ex, "[AuthHttpClient] Error processing response: {Message}", ex.Message);

            result.Success = false;
            result.Message = $"Error processing response: {ex.Message}";
        }

        return result;
    }

    private AuthResponseDto<T> CreateErrorResponse<T>(Exception ex, string operation)
    {
        _logger.LogError(ex, "[AuthHttpClient] Error in {Operation}: {Message}", operation, ex.Message);

        return new AuthResponseDto<T>
        {
            Success = false,
            Message = $"Request error [{operation}]: {ex.Message}"
        };
    }
}

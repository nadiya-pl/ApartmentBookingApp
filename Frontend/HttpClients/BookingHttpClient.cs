using Frontend.Exceptions;
using Frontend.Models;
using Frontend.Token;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Frontend.HttpClients;

public class BookingHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly ITokenManager _tokenProvider;
    private readonly ILogger<BookingHttpClient> _logger;

    public BookingHttpClient(HttpClient httpClient, ITokenManager tokenProvider,  ILogger<BookingHttpClient> logger)
    {
        _httpClient = httpClient;
        _tokenProvider = tokenProvider;
        _logger = logger;

    }

    public async Task<ApiResponseDto<List<PropertyDto>>> GetAllProperties()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/properties");
            return await HandleResponse<List<PropertyDto>>(response);
        }
        catch(Exception ex)
        {
            return CreateErrorResponse<List<PropertyDto>>(ex, "GetAllProperties");
        }
    }
     
    public async Task<ApiResponseDto<PropertyDto>> GetProperty(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync("api/properties/" + id);
            return await HandleResponse<PropertyDto>(response);
        }
        catch (Exception ex)
        {
            return CreateErrorResponse<PropertyDto>(ex, "GetProperty");
        }
    }

    public async Task<ApiResponseDto<BookingDto>> GetBookingById(int id)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/booking/" + id);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenProvider.GetAccessToken());

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            return await HandleResponse<BookingDto>(response);
        }
        catch (Exception ex)
        {
            return CreateErrorResponse<BookingDto>(ex, "GetBookingById");
        }
    }

    public async Task<ApiResponseDto<PropertyDto>> AddNewProperty(AddPropertyDto propertyDto)
    {
        try
        {
            var json = JsonSerializer.Serialize(propertyDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
                     
            var request = new HttpRequestMessage(HttpMethod.Post, "api/properties");
            request.Content = content;                   
            var token = _tokenProvider.GetAccessToken();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        
            HttpResponseMessage response = await _httpClient.SendAsync(request);            
            return await HandleResponse<PropertyDto>(response);
        }
        catch (ApiUnauthorizedException)
        {
            throw;
        }
        catch (ApiForbiddenException)
        {
            throw;
        }
        catch (Exception ex)
        {
            return CreateErrorResponse<PropertyDto>(ex, "AddNewProperty");
        }
    }

    public async Task<ApiResponseDto<object>> DeleteProperty(int id)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"api/properties/{id}");          
            var token = _tokenProvider.GetAccessToken();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                
            HttpResponseMessage response = await _httpClient.SendAsync(request); 
            return await HandleResponse<object>(response);
        }
        catch (ApiUnauthorizedException)
        {
            throw;
        }
        catch (ApiForbiddenException)
        {
            throw;
        }
        catch (Exception ex)
        {
            return CreateErrorResponse<object>(ex, "DeleteProperty");
        }
    }

     public async Task<ApiResponseDto<BookingDto>> AddNewBooking(AddBookingDto bookingDto)
    {
        try
        {
            var json = JsonSerializer.Serialize(bookingDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
                  
            var request = new HttpRequestMessage(HttpMethod.Post, "api/booking");
            request.Content = content;        
            var token = _tokenProvider.GetAccessToken();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
              
            HttpResponseMessage response = await _httpClient.SendAsync(request);      
            return await HandleResponse<BookingDto>(response);
        }
        catch (ApiUnauthorizedException)
        {
            throw;
        }
        catch (ApiForbiddenException)
        {
            throw;
        }
        catch (Exception ex)
        {
            return CreateErrorResponse<BookingDto>(ex, "AddNewBooking");
        }
    }

    public async Task<ApiResponseDto<List<BookingDto>>> GetAllBookings()
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/booking");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenProvider.GetAccessToken());

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            return await HandleResponse<List<BookingDto>>(response);
        }
        catch (ApiUnauthorizedException)
        {
            throw;
        }
        catch (ApiForbiddenException)
        {
            throw;
        }
        catch (Exception ex)
        {
            return CreateErrorResponse<List<BookingDto>>(ex, "GetAllBookings");
        }
    }

    private async Task<ApiResponseDto<T>> HandleResponse<T>(HttpResponseMessage response)
    {
        if (response.StatusCode == HttpStatusCode.Unauthorized)
            throw new ApiUnauthorizedException();

        if (response.StatusCode == HttpStatusCode.Forbidden)
            throw new ApiForbiddenException();

        var result = new ApiResponseDto<T>();

        try
        {
            var temp = await response.Content.ReadFromJsonAsync<ApiResponseDto<JsonElement>>();

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
            result.Data = temp.Success ? (typeof(T) == typeof(object) ? (T)(object)temp.Data : temp.Data.Deserialize<T>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true })) : default;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Message = $"Error processing response: {ex.Message}";
            _logger.LogError(ex, "[BookingHttpClient] Error processing response: {Message}", ex.Message);
        }

        return result;
    }

    private ApiResponseDto<T> CreateErrorResponse<T>(Exception ex, string operation)
    {
        _logger.LogError(ex, "[BookingHttpClient] Error in {Operation}: {Message}", operation, ex.Message);

        return new ApiResponseDto<T>
        {
            Success = false,
            Message = $"Request error [{operation}]: {ex.Message}"
        };
    }
}

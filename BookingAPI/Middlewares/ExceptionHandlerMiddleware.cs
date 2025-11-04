using BookingAPI.Models.Dto;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BookingAPI.Middlewares;


public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (OperationCanceledException)
        {
            if (!httpContext.Response.HasStarted)
                httpContext.Response.StatusCode = 499;
        }
        catch (Exception ex)
        {
            if (httpContext.Response.HasStarted)
            {
                throw;
            }

            _logger.LogError(ex, "Unhandled exception {TraceId}", httpContext.TraceIdentifier);

            var (status, title) = MapException(ex);


            // формирование ответа 
            httpContext.Response.Clear();
            httpContext.Response.StatusCode = status;
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.Headers["X-Trace-Id"] = httpContext.TraceIdentifier;

            // если это девелопмент, то отдать все детали ошибки
            var message = _env.IsDevelopment() ? $"{title}: {ex.Message}" : title;

            var response = new ApiResponseDto();
            response.Success = false;
            response.Message = message;
            response.TraceId = httpContext.TraceIdentifier;
            await httpContext.Response.WriteAsJsonAsync(response);
        }
    }

    private (int StatusCode, string Title) MapException(Exception ex)
    {
        if (ex is KeyNotFoundException)
        {
            return (StatusCodes.Status404NotFound, "Resource not found");
        }
        else if (ex is ArgumentException)
        {
            return (StatusCodes.Status400BadRequest, "Bad request");
        }
        else if (ex is UnauthorizedAccessException)
        {
            return (StatusCodes.Status401Unauthorized, "Unauthorized");
        }
        else if (ex is DbUpdateConcurrencyException)
        {
            return (StatusCodes.Status409Conflict, "Concurrency conflict");
        }
        else
        {
            return (StatusCodes.Status500InternalServerError, "Unexpected error");
        }
    }
}


public static class ExceptionHandlerMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}

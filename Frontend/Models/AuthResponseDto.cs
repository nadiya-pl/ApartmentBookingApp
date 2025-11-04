using System.Text.Json.Serialization;

namespace Frontend.Models;

public class AuthResponseDto<T>
{
    public bool Success { get; set; }
    public T Data { get; set; }

    public string Message { get; set; }

    public string TraceId { get; set; }

    public Dictionary<string, string> Errors { get; set; }
}

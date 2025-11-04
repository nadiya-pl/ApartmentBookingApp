namespace Frontend.Exceptions;

public sealed class ApiUnauthorizedException : Exception
{
    public ApiUnauthorizedException() : base("API returned 401") { }
}
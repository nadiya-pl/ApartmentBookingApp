namespace Frontend.Exceptions;

public sealed class ApiForbiddenException : Exception
{
    public ApiForbiddenException() : base("API returned 403") { }
}

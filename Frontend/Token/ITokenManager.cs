namespace Frontend.Token;

public interface ITokenManager
{
    string? GetAccessToken();
    void SetAccessToken(string token);
    string? GetRefreshToken();
    void SetRefreshToken(string refreshToken);
    void ClearTokens();
}

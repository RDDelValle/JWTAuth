namespace JWTAuth.Data.Auth;

public interface IAuthService
{
    Task<AuthResult> LoginAsync(string username, string password);
    Task<AuthResult> RevokeAsync(string username);
    Task<AuthResult> RefreshAsync(string authToken, string refreshToken);
}
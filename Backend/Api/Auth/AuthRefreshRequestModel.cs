namespace JWTAuth.Api.Auth;

public sealed class AuthRefreshRequestModel
{
    public required string AuthToken { get; set; }
    public required string RefreshToken { get; set; }
}
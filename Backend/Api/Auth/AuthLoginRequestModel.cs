namespace JWTAuth.Api.Auth;

public sealed class AuthLoginRequestModel
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
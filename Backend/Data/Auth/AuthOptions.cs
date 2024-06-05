using Microsoft.IdentityModel.Tokens;

namespace JWTAuth.Data.Auth;

public class AuthOptions
{
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public required SecurityKey SecurityKey  { get; set; }
    public required int Expiration { get; set; }
    public required int RefreshExpiration { get; set; }

    public DateTime ExpirationDate
        => DateTime.UtcNow.AddSeconds(Expiration);
    
    public DateTime RefreshExpirationDate
        => DateTime.UtcNow.AddSeconds(RefreshExpiration);
}
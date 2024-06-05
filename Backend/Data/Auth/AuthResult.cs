namespace JWTAuth.Data.Auth;

public class AuthResult
{
    private AuthResult()
    {
        Successful = true;
    }
    
    private AuthResult(string token, DateTime expiration, string refreshToken)
    {
        Successful = true;
        AuthToken = token;
        RefreshToken = refreshToken;
        Expiration = expiration;
    }
    
    private AuthResult(string[] errors)
    {
        if (errors.Length == 0)
            errors = new[] { "An unknown error has occurred." };
        
        Successful = false;
        Errors = errors;
    }
    
    public string? AuthToken { get; private set; }
    public string? RefreshToken { get; private set; }
    public DateTime? Expiration { get; private set; }
    
    public bool Successful { get; private set; }
    public IReadOnlyList<string>? Errors { get; private set; }
    
    public static AuthResult Success()
        => new AuthResult();
    
    public static AuthResult Success(string token, DateTime expiration, string refreshToken)
        => new AuthResult(token, expiration, refreshToken);

    public static AuthResult Failure(params string[] errors)
        => new AuthResult(errors);
}
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using JWTAuth.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JWTAuth.Data.Auth;

internal class AuthService(UserManager<ApplicationUser> userManager, IOptions<AuthOptions> options) : IAuthService
{
    public async Task<AuthResult> LoginAsync(string username, string password)
    {
        var user = await userManager.FindByNameAsync(username);
        if (user is null || !await userManager.CheckPasswordAsync(user, password))
            return AuthResult.Failure("Invalid username and/or password");

        var token = GenerateJwt(username);
        user.RefreshToken = GenerateRefreshToken();
        user.RefreshTokenExpiration = options.Value.RefreshExpirationDate;

        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
            return AuthResult.Failure(result.Errors.Select(e => e.Code).ToArray());

        var authResult = AuthResult.Success(
            new JwtSecurityTokenHandler().WriteToken(token),
            token.ValidTo,
            user.RefreshToken);
        return authResult;
    }

    public async Task<AuthResult> RefreshAsync(string authToken, string refreshToken)
    {
        var principal = GetPrincipalFromExpiredToken(authToken);

        if (principal?.Identity?.Name is null)
            return AuthResult.Failure("Invalid Token");

        var user = await userManager.FindByNameAsync(principal.Identity.Name);

        if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiration < DateTime.UtcNow)
            return AuthResult.Failure("Invalid Token");
        
        var token = GenerateJwt(principal.Identity.Name);

        // if refresh token does not expires before token expires
        if (!(user.RefreshTokenExpiration < DateTime.UtcNow.AddSeconds(options.Value.Expiration)))
            return AuthResult.Success(
                new JwtSecurityTokenHandler().WriteToken(token),
                token.ValidTo,
                user.RefreshToken);
        
        // refresh token expires before the token, then we need to refresh it as well.
        user.RefreshToken = GenerateRefreshToken();
        user.RefreshTokenExpiration = DateTime.UtcNow.AddSeconds(options.Value.RefreshExpiration);
        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
            return AuthResult.Failure(result.Errors.Select(e => e.Code).ToArray());

        return AuthResult.Success(
            new JwtSecurityTokenHandler().WriteToken(token),
            token.ValidTo,
            user.RefreshToken);
    }
    
    public async Task<AuthResult> RevokeAsync(string username)
    {
        var user = await userManager.FindByNameAsync(username);
        if (user?.RefreshToken is null)
            return AuthResult.Failure("Invalid username.");
        
        user.RefreshToken = null;
        user.RefreshTokenExpiration = null;
        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
            return AuthResult.Failure(result.Errors.Select(e => e.Code).ToArray());
        return AuthResult.Success();
    }
    
    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var validation = new TokenValidationParameters
        {
            ValidIssuer = options.Value.Issuer,
            ValidAudience = options.Value.Audience,
            IssuerSigningKey = options.Value.SecurityKey,
            ValidateLifetime = false
        };
        return new JwtSecurityTokenHandler().ValidateToken(token, validation, out _);
    }

    private JwtSecurityToken GenerateJwt(string username)
    {
        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: options.Value.Issuer,
            audience: options.Value.Audience,
            expires: DateTime.UtcNow.AddSeconds(options.Value.Expiration),
            claims: authClaims,
            signingCredentials: new SigningCredentials(options.Value.SecurityKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];

        using var generator = RandomNumberGenerator.Create();

        generator.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }
}
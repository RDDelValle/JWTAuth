using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace JWTAuth.Data.Auth;

public static class AuthExtensions
{
    internal static WebApplicationBuilder AddAuthService(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAuthService, AuthService>();
        
        var configuration = builder.Configuration.GetSection("Auth").Get<AuthConfiguration>()
                            ?? throw new InvalidOperationException("Auth not configured.");
        
        builder.Services.Configure<AuthOptions>(options =>
        {
            options.Issuer = configuration.Issuer;
            options.Audience = configuration.Audience;
            options.SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.Key));
            options.Expiration = configuration.Expiration;
            options.RefreshExpiration = configuration.RefreshExpiration;
        });

        return builder;
    }
}
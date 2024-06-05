using JWTAuth.Data.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuth.Api.Auth;

[ApiController]
[Route("[controller]")]
public class AuthController(ILogger<AuthController> logger, IAuthService authService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<IResult> LoginAsync([FromBody] AuthLoginRequestModel model)
    {
        logger.LogInformation("Auth/Login called!");
        if (!ModelState.IsValid)
        {
            logger.LogWarning("Login Failed: Invalid Model State!");
            return Results.Unauthorized();
        }
        
        var result = await authService.LoginAsync(model.Email, model.Password);
        if (!result.Successful)
        {
            logger.LogWarning("Login Failed!: Invalid Credentials!");
            return Results.Unauthorized();
        }
        
        logger.LogInformation("Login Succeeded!");
        return Results.Ok(new {result.AuthToken, result.Expiration, result.RefreshToken});
    }
    
    [AllowAnonymous]
    [HttpPost("Refresh")]
    public async Task<IResult> RefreshAsync([FromBody] AuthRefreshRequestModel model)
    {
        logger.LogInformation("Auth/Refresh called!");
        if (!ModelState.IsValid)
        {
            logger.LogWarning("Refresh Failed: Invalid Model State!");
            return Results.Unauthorized();
        }
        
        var result = await authService.RefreshAsync(model.AuthToken, model.RefreshToken);
        if (!result.Successful)
        {
            logger.LogWarning("Refresh Failed: Invalid Credentials!");
            return Results.Unauthorized();
        }
        
        logger.LogInformation("Refresh Succeeded!");
        return Results.Ok(new {result.AuthToken, result.Expiration, result.RefreshToken});
    }
    
    [Authorize(Policy = AuthConstants.AuthenticatedUser)]
    [HttpDelete("Revoke")]
    public async Task<IResult> RevokeAsync()
    {
        logger.LogInformation("Auth/Revoke called!");
        var username = User.Identity?.Name;
        if (username is null)
        {
            logger.LogWarning("Revoke Failed: null user");
            return Results.Unauthorized();
        }
        
        var result = await authService.RevokeAsync(username);

        if (!result.Successful)
        {
            logger.LogWarning("Revoke Failed: Already revoked.");
            return Results.Unauthorized();
        }
        
        logger.LogInformation("Revoke Succeeded");
        return Results.Ok();
    }
}

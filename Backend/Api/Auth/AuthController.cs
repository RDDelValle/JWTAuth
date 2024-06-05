using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuth.Api.Auth;

[ApiController]
[Route("[controller]")]
public class AuthController(ILogger<AuthController> logger) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<IResult> LoginAsync([FromBody] AuthLoginRequestModel model)
    {
        logger.LogInformation("Auth/Login called!");
        await Task.CompletedTask;
        return Results.Ok();
    }
    
    [AllowAnonymous]
    [HttpPost("Refresh")]
    public async Task<IResult> RefreshAsync([FromBody] AuthRefreshRequestModel model)
    {
        logger.LogInformation("Auth/Refresh called!");
        await Task.CompletedTask;
        return Results.Ok();
    }
    
    [Authorize]
    [HttpDelete(Name = "Revoke")]
    public async Task<IResult> RevokeAsync()
    {
        logger.LogInformation("Auth/Revoke called!");
        await Task.CompletedTask;
        return Results.Ok();
    }
}

using System.ComponentModel.DataAnnotations;

namespace JWTAuth.Api.Auth;

public sealed class AuthLoginRequestModel
{
    [EmailAddress][MinLength(6)][MaxLength(32)]public required string Email { get; set; }
    [MinLength(8)][MaxLength(32)]public required string Password { get; set; }
}
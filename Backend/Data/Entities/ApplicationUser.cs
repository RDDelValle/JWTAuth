using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace JWTAuth.Data.Entities;

public class ApplicationUser : IdentityUser
{
    [MaxLength(256)]public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiration { get; set; }
}
using JWTAuth.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JWTAuth.Data.Seed;

internal static class IdentitySeed
{
    public static void SeedIdentity(this ModelBuilder builder)
    {
        // Create User
        const string userId = "62cf5133-fb72-4cc1-9bea-ff8bc6c21f01";
        const string userEmail = "user@domain.com";
        const string userPassword = "Password*1";
        var user = new ApplicationUser()
        {
            Id = userId,
            Email = userEmail,
            NormalizedEmail = userEmail.ToUpper(),
            EmailConfirmed = true,
            UserName = userEmail,
            NormalizedUserName = userEmail.ToUpper()
        };

        //  Set user password
        var passwordHasher = new PasswordHasher<ApplicationUser>();
        user.PasswordHash = passwordHasher.HashPassword(user, userPassword);

        // Seed User
        builder.Entity<ApplicationUser>()
            .HasData(user);
    }
}
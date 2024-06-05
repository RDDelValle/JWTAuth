using JWTAuth.Data.Context;
using JWTAuth.Data.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JWTAuth.Data;

public static class Extensions
{
    public static WebApplicationBuilder AddData(this WebApplicationBuilder builder)
        => builder.AddDataContext().AddDataIdentity();
    
    private static WebApplicationBuilder AddDataContext(this WebApplicationBuilder builder)
    {
        bool isDevelopment = builder.Environment.IsDevelopment();
        string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                                  ?? throw new InvalidOperationException("DefaultConnection not configured.");

        Action<DbContextOptionsBuilder> dbOptions = isDevelopment
            ? options => options.UseSqlite(connectionString)
            : options => options.UseSqlServer(connectionString);

        builder.Services.AddDbContext<ApplicationDbContext>(dbOptions);
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        return builder;
    }
    
    private static WebApplicationBuilder AddDataIdentity(this WebApplicationBuilder builder)
    {
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedAccount = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        return builder;
    }
}
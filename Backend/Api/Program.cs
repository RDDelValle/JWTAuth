using System.Text;
using JWTAuth.Api.Auth;
using JWTAuth.Data;
using JWTAuth.Data.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.AddData();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var authConfiguration = builder.Configuration.GetSection("Auth").Get<AuthConfiguration>()
                        ?? throw new InvalidOperationException("Auth not configured.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = authConfiguration.Issuer,
        ValidAudience = authConfiguration.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfiguration.Key)),
        ClockSkew = new TimeSpan(0, 0, 5)
    };
});

builder.Services.AddAuthorizationBuilder()
    .AddPolicy(AuthConstants.AuthenticatedUser, p =>
    {
        p.RequireAuthenticatedUser();
        p.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
    });

const string corsPolicy = "clientCors";
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicy,
        p =>
        {
            p.AllowAnyOrigin();
            p.AllowAnyHeader();
            p.AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(corsPolicy);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JWTAuth.Data.Migrations.SeedUser
{
    /// <inheritdoc />
    public partial class SeedUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiration", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "62cf5133-fb72-4cc1-9bea-ff8bc6c21f01", 0, "f2531cc3-fa7f-4a08-9923-c5a88c485224", "user@domain.com", true, false, null, "USER@DOMAIN.COM", "USER@DOMAIN.COM", "AQAAAAIAAYagAAAAEJtv6uvrynxM1q8GnC4f76UNIDaKdybiPcskjOt2vLsdDMu3hZ1KJFKkH7wW8/tdBA==", null, false, null, null, "15081b5d-462f-4ac6-a0b6-3f4942f990d4", false, "user@domain.com" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "62cf5133-fb72-4cc1-9bea-ff8bc6c21f01");
        }
    }
}

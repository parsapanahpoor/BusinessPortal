using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessPortal.Data.Migrations
{
    public partial class ChangeEmailSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "EmailSettings",
                keyColumn: "Id",
                keyValue: 1m,
                columns: new[] { "DisplayName", "From", "Password", "UserName" },
                values: new object[] { "BusinessPortal", "maghsoudlou.reza@gmail.com", "Reza@83040697", "BusinessPortal" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "EmailSettings",
                keyColumn: "Id",
                keyValue: 1m,
                columns: new[] { "DisplayName", "From", "Password", "UserName" },
                values: new object[] { "academy", "barnamenevisan.academy@gmail.com", "password@strong", "academy" });
        }
    }
}

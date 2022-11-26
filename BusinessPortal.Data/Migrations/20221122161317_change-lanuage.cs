using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessPortal.Data.Migrations
{
    public partial class changelanuage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Language",
                keyColumn: "LanguageTitle",
                keyValue: "fa-IR");

            migrationBuilder.InsertData(
                table: "Language",
                column: "LanguageTitle",
                value: "bg-BG");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Language",
                keyColumn: "LanguageTitle",
                keyValue: "bg-BG");

            migrationBuilder.InsertData(
                table: "Language",
                column: "LanguageTitle",
                value: "fa-IR");
        }
    }
}

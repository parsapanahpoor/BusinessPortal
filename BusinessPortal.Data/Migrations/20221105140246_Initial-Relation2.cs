using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessPortal.Data.Migrations
{
    public partial class InitialRelation2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Advertisement_CountriesId",
                table: "Advertisement");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisement_CountriesId",
                table: "Advertisement",
                column: "CountriesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Advertisement_CountriesId",
                table: "Advertisement");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisement_CountriesId",
                table: "Advertisement",
                column: "CountriesId",
                unique: true,
                filter: "[CountriesId] IS NOT NULL");
        }
    }
}

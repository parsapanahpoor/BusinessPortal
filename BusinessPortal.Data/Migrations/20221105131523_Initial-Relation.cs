using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessPortal.Data.Migrations
{
    public partial class InitialRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CountriesId",
                table: "Advertisement",
                type: "decimal(20,0)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Advertisement_CountriesId",
                table: "Advertisement",
                column: "CountriesId",
                unique: true,
                filter: "[CountriesId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisement_Countries_CountriesId",
                table: "Advertisement",
                column: "CountriesId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advertisement_Countries_CountriesId",
                table: "Advertisement");

            migrationBuilder.DropIndex(
                name: "IX_Advertisement_CountriesId",
                table: "Advertisement");

            migrationBuilder.DropColumn(
                name: "CountriesId",
                table: "Advertisement");
        }
    }
}

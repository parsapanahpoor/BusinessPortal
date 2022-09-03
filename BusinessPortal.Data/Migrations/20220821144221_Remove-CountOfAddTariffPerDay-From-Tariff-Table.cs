using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessPortal.Data.Migrations
{
    public partial class RemoveCountOfAddTariffPerDayFromTariffTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountOfAddAdvertisement",
                table: "Tariffs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountOfAddAdvertisement",
                table: "Tariffs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessPortal.Data.Migrations
{
    public partial class InitialTariffTable2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tariffs",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TariffName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TariffPrice = table.Column<int>(type: "int", nullable: false),
                    CountOfSeenAdvertisement = table.Column<int>(type: "int", nullable: false),
                    CountOfAddAdvertisement = table.Column<int>(type: "int", nullable: false),
                    tariffDuration = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tariffs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tariffs");
        }
    }
}

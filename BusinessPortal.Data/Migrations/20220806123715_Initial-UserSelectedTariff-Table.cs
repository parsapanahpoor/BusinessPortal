using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessPortal.Data.Migrations
{
    public partial class InitialUserSelectedTariffTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserSelectedTariff",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TariffId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    UserId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    Startdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSelectedTariff", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSelectedTariff_Tariffs_TariffId",
                        column: x => x.TariffId,
                        principalTable: "Tariffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserSelectedTariff_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSelectedTariff_TariffId",
                table: "UserSelectedTariff",
                column: "TariffId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSelectedTariff_UserId",
                table: "UserSelectedTariff",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSelectedTariff");
        }
    }
}

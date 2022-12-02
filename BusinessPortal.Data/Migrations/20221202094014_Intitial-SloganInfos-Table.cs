using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessPortal.Data.Migrations
{
    public partial class IntitialSloganInfosTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Slogans",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slogans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SloganInfos",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LanguageId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    SloganId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SloganInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SloganInfos_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Language",
                        principalColumn: "LanguageTitle",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SloganInfos_Slogans_SloganId",
                        column: x => x.SloganId,
                        principalTable: "Slogans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SloganInfos_LanguageId",
                table: "SloganInfos",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_SloganInfos_SloganId",
                table: "SloganInfos",
                column: "SloganId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SloganInfos");

            migrationBuilder.DropTable(
                name: "Slogans");
        }
    }
}

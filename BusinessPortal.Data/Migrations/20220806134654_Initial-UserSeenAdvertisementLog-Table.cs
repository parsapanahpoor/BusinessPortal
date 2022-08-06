using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessPortal.Data.Migrations
{
    public partial class InitialUserSeenAdvertisementLogTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserCreateAdvertisementLogs",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    FromCustomer = table.Column<bool>(type: "bit", nullable: false),
                    FromEmployee = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCreateAdvertisementLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCreateAdvertisementLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserSeenAdvertisementLogs",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSeenAdvertisementLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSeenAdvertisementLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserCreateAdvertisementLogs_UserId",
                table: "UserCreateAdvertisementLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSeenAdvertisementLogs_UserId",
                table: "UserSeenAdvertisementLogs",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserCreateAdvertisementLogs");

            migrationBuilder.DropTable(
                name: "UserSeenAdvertisementLogs");
        }
    }
}

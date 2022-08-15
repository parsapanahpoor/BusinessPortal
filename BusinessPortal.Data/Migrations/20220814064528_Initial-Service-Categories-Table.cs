using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessPortal.Data.Migrations
{
    public partial class InitialServiceCategoriesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServicesCategories",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UniqueName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ParentId = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    ParentId1 = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicesCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServicesCategories_ServicesCategories_ParentId1",
                        column: x => x.ParentId1,
                        principalTable: "ServicesCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServicesCategoryInfos",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LanguageId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    ArticleCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: false),
                    ServicesCategoryId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicesCategoryInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServicesCategoryInfos_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Language",
                        principalColumn: "LanguageTitle",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServicesCategoryInfos_ServicesCategories_ServicesCategoryId",
                        column: x => x.ServicesCategoryId,
                        principalTable: "ServicesCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServicesCategories_ParentId1",
                table: "ServicesCategories",
                column: "ParentId1");

            migrationBuilder.CreateIndex(
                name: "IX_ServicesCategoryInfos_LanguageId",
                table: "ServicesCategoryInfos",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicesCategoryInfos_ServicesCategoryId",
                table: "ServicesCategoryInfos",
                column: "ServicesCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServicesCategoryInfos");

            migrationBuilder.DropTable(
                name: "ServicesCategories");
        }
    }
}

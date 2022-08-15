using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessPortal.Data.Migrations
{
    public partial class UpdateServiceCategoriesTable2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArticleCategoryId",
                table: "ServicesCategoryInfos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ArticleCategoryId",
                table: "ServicesCategoryInfos",
                type: "decimal(20,0)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}

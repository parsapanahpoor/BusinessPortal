using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessPortal.Data.Migrations
{
    public partial class UpdateServiceCategoriesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServicesCategories_ServicesCategories_ParentId1",
                table: "ServicesCategories");

            migrationBuilder.DropIndex(
                name: "IX_ServicesCategories_ParentId1",
                table: "ServicesCategories");

            migrationBuilder.DropColumn(
                name: "ParentId1",
                table: "ServicesCategories");

            migrationBuilder.AlterColumn<decimal>(
                name: "ArticleCategoryId",
                table: "ServicesCategoryInfos",
                type: "decimal(20,0)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<decimal>(
                name: "ParentId",
                table: "ServicesCategories",
                type: "decimal(20,0)",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServicesCategories_ParentId",
                table: "ServicesCategories",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServicesCategories_ServicesCategories_ParentId",
                table: "ServicesCategories",
                column: "ParentId",
                principalTable: "ServicesCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServicesCategories_ServicesCategories_ParentId",
                table: "ServicesCategories");

            migrationBuilder.DropIndex(
                name: "IX_ServicesCategories_ParentId",
                table: "ServicesCategories");

            migrationBuilder.AlterColumn<long>(
                name: "ArticleCategoryId",
                table: "ServicesCategoryInfos",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,0)");

            migrationBuilder.AlterColumn<long>(
                name: "ParentId",
                table: "ServicesCategories",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,0)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ParentId1",
                table: "ServicesCategories",
                type: "decimal(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_ServicesCategories_ParentId1",
                table: "ServicesCategories",
                column: "ParentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ServicesCategories_ServicesCategories_ParentId1",
                table: "ServicesCategories",
                column: "ParentId1",
                principalTable: "ServicesCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

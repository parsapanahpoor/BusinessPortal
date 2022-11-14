using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessPortal.Data.Migrations
{
    public partial class InitialProductServiceTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Lang_Id",
                table: "AdsInfo",
                type: "nvarchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "ProductService",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddressId = table.Column<decimal>(type: "decimal(20,0)", nullable: true),
                    UserId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    ImageName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductService_States_AddressId",
                        column: x => x.AddressId,
                        principalTable: "States",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductService_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductServiceInfo",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductServiceId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    Lang_Id = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductServiceInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductServiceInfo_Language_Lang_Id",
                        column: x => x.Lang_Id,
                        principalTable: "Language",
                        principalColumn: "LanguageTitle",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductServiceInfo_ProductService_ProductServiceId",
                        column: x => x.ProductServiceId,
                        principalTable: "ProductService",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductServiceSelectedService",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductServiceId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    ServiceId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductServiceSelectedService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductServiceSelectedService_ProductService_ProductServiceId",
                        column: x => x.ProductServiceId,
                        principalTable: "ProductService",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductServiceSelectedService_ServicesCategories_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "ServicesCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdsInfo_Lang_Id",
                table: "AdsInfo",
                column: "Lang_Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductService_AddressId",
                table: "ProductService",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductService_UserId",
                table: "ProductService",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductServiceInfo_Lang_Id",
                table: "ProductServiceInfo",
                column: "Lang_Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductServiceInfo_ProductServiceId",
                table: "ProductServiceInfo",
                column: "ProductServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductServiceSelectedService_ProductServiceId",
                table: "ProductServiceSelectedService",
                column: "ProductServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductServiceSelectedService_ServiceId",
                table: "ProductServiceSelectedService",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdsInfo_Language_Lang_Id",
                table: "AdsInfo",
                column: "Lang_Id",
                principalTable: "Language",
                principalColumn: "LanguageTitle",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdsInfo_Language_Lang_Id",
                table: "AdsInfo");

            migrationBuilder.DropTable(
                name: "ProductServiceInfo");

            migrationBuilder.DropTable(
                name: "ProductServiceSelectedService");

            migrationBuilder.DropTable(
                name: "ProductService");

            migrationBuilder.DropIndex(
                name: "IX_AdsInfo_Lang_Id",
                table: "AdsInfo");

            migrationBuilder.AlterColumn<string>(
                name: "Lang_Id",
                table: "AdsInfo",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");
        }
    }
}

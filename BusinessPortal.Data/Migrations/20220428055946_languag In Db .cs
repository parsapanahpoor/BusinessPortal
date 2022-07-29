using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessPortal.Data.Migrations
{
    public partial class languagInDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdvertisementInfo_Advertisement_AdvertisementId",
                table: "AdvertisementInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_AdvertisementInfo_Language_Lang_Id",
                table: "AdvertisementInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AdvertisementInfo",
                table: "AdvertisementInfo");

            migrationBuilder.RenameTable(
                name: "AdvertisementInfo",
                newName: "advertisementInfo");

            migrationBuilder.RenameIndex(
                name: "IX_AdvertisementInfo_Lang_Id",
                table: "advertisementInfo",
                newName: "IX_advertisementInfo_Lang_Id");

            migrationBuilder.RenameIndex(
                name: "IX_AdvertisementInfo_AdvertisementId",
                table: "advertisementInfo",
                newName: "IX_advertisementInfo_AdvertisementId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_advertisementInfo",
                table: "advertisementInfo",
                column: "Id");

            migrationBuilder.InsertData(
                table: "Language",
                column: "LanguageTitle",
                values: new object[]
                {
                    "ar-SA",
                    "en-US",
                    "fa-IR",
                    "pt-PT",
                    "ru-RU",
                    "tr-TR"
                });

            migrationBuilder.AddForeignKey(
                name: "FK_advertisementInfo_Advertisement_AdvertisementId",
                table: "advertisementInfo",
                column: "AdvertisementId",
                principalTable: "Advertisement",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_advertisementInfo_Language_Lang_Id",
                table: "advertisementInfo",
                column: "Lang_Id",
                principalTable: "Language",
                principalColumn: "LanguageTitle",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_advertisementInfo_Advertisement_AdvertisementId",
                table: "advertisementInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_advertisementInfo_Language_Lang_Id",
                table: "advertisementInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_advertisementInfo",
                table: "advertisementInfo");

            migrationBuilder.DeleteData(
                table: "Language",
                keyColumn: "LanguageTitle",
                keyValue: "ar-SA");

            migrationBuilder.DeleteData(
                table: "Language",
                keyColumn: "LanguageTitle",
                keyValue: "en-US");

            migrationBuilder.DeleteData(
                table: "Language",
                keyColumn: "LanguageTitle",
                keyValue: "fa-IR");

            migrationBuilder.DeleteData(
                table: "Language",
                keyColumn: "LanguageTitle",
                keyValue: "pt-PT");

            migrationBuilder.DeleteData(
                table: "Language",
                keyColumn: "LanguageTitle",
                keyValue: "ru-RU");

            migrationBuilder.DeleteData(
                table: "Language",
                keyColumn: "LanguageTitle",
                keyValue: "tr-TR");

            migrationBuilder.RenameTable(
                name: "advertisementInfo",
                newName: "AdvertisementInfo");

            migrationBuilder.RenameIndex(
                name: "IX_advertisementInfo_Lang_Id",
                table: "AdvertisementInfo",
                newName: "IX_AdvertisementInfo_Lang_Id");

            migrationBuilder.RenameIndex(
                name: "IX_advertisementInfo_AdvertisementId",
                table: "AdvertisementInfo",
                newName: "IX_AdvertisementInfo_AdvertisementId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdvertisementInfo",
                table: "AdvertisementInfo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AdvertisementInfo_Advertisement_AdvertisementId",
                table: "AdvertisementInfo",
                column: "AdvertisementId",
                principalTable: "Advertisement",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AdvertisementInfo_Language_Lang_Id",
                table: "AdvertisementInfo",
                column: "Lang_Id",
                principalTable: "Language",
                principalColumn: "LanguageTitle",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

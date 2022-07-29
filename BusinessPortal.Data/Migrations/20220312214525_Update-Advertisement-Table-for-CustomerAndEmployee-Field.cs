using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessPortal.Data.Migrations
{
    public partial class UpdateAdvertisementTableforCustomerAndEmployeeField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "FromCustomer",
                table: "Advertisement",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "FromEmployee",
                table: "Advertisement",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromCustomer",
                table: "Advertisement");

            migrationBuilder.DropColumn(
                name: "FromEmployee",
                table: "Advertisement");
        }
    }
}

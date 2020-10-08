using Microsoft.EntityFrameworkCore.Migrations;

namespace Whs.Server.Data.Migrations
{
    public partial class WhsOrderData_Remake : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NewStatus",
                table: "WhsOrdersDataOut");

            migrationBuilder.DropColumn(
                name: "NewStatus",
                table: "WhsOrdersDataIn");

            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "StatusSwitchDate",
                table: "WhsOrdersDataOut",
                newName: "DateTime");

            migrationBuilder.RenameColumn(
                name: "OldStatus",
                table: "WhsOrdersDataOut",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "StatusSwitchDate",
                table: "WhsOrdersDataIn",
                newName: "DateTime");

            migrationBuilder.RenameColumn(
                name: "OldStatus",
                table: "WhsOrdersDataIn",
                newName: "Status");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "WhsOrdersDataOut",
                newName: "StatusSwitchDate");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "WhsOrdersDataOut",
                newName: "OldStatus");

            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "WhsOrdersDataIn",
                newName: "StatusSwitchDate");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "WhsOrdersDataIn",
                newName: "OldStatus");

            migrationBuilder.AddColumn<string>(
                name: "NewStatus",
                table: "WhsOrdersDataOut",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NewStatus",
                table: "WhsOrdersDataIn",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

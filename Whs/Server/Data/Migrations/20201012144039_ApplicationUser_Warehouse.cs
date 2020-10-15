using Microsoft.EntityFrameworkCore.Migrations;

namespace Whs.Server.Data.Migrations
{
    public partial class ApplicationUser_Warehouse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            //migrationBuilder.AddColumn<string>(
            //    name: "WarehouseId",
            //    table: "AspNetUsers",
            //    nullable: true);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_AspNetUsers_Warehouses_WarehouseId",
            //    table: "AspNetUsers",
            //    column: "WarehouseId",
            //    principalTable: "Warehouses",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_AspNetUsers_Warehouses_WarehouseId",
            //    table: "AspNetUsers");

            //migrationBuilder.DropColumn(
            //   name: "WarehouseId",
            //   table: "AspNetUsers");
        }
    }
}

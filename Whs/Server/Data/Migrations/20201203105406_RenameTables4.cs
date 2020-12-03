using Microsoft.EntityFrameworkCore.Migrations;

namespace Whs.Server.Data.Migrations
{
    public partial class RenameTables4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Warehouses_WarehouseId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_WhsProductsDataIn_EditingCauses_EditingCauseId",
                table: "WhsProductsDataIn");

            migrationBuilder.DropForeignKey(
                name: "FK_WhsProductsDataOut_EditingCauses_EditingCauseId",
                table: "WhsProductsDataOut");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Warehouses",
                table: "Warehouses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EditingCauses",
                table: "EditingCauses");

            migrationBuilder.RenameTable(
                name: "Warehouses",
                newName: "WhsWarehouse");

            migrationBuilder.RenameTable(
                name: "EditingCauses",
                newName: "WhsEditingCauses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WhsWarehouse",
                table: "WhsWarehouse",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WhsEditingCauses",
                table: "WhsEditingCauses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_WhsWarehouse_WarehouseId",
                table: "AspNetUsers",
                column: "WarehouseId",
                principalTable: "WhsWarehouse",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WhsProductsDataIn_WhsEditingCauses_EditingCauseId",
                table: "WhsProductsDataIn",
                column: "EditingCauseId",
                principalTable: "WhsEditingCauses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_WhsProductsDataOut_WhsEditingCauses_EditingCauseId",
                table: "WhsProductsDataOut",
                column: "EditingCauseId",
                principalTable: "WhsEditingCauses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_WhsWarehouse_WarehouseId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_WhsProductsDataIn_WhsEditingCauses_EditingCauseId",
                table: "WhsProductsDataIn");

            migrationBuilder.DropForeignKey(
                name: "FK_WhsProductsDataOut_WhsEditingCauses_EditingCauseId",
                table: "WhsProductsDataOut");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WhsWarehouse",
                table: "WhsWarehouse");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WhsEditingCauses",
                table: "WhsEditingCauses");

            migrationBuilder.RenameTable(
                name: "WhsWarehouse",
                newName: "Warehouses");

            migrationBuilder.RenameTable(
                name: "WhsEditingCauses",
                newName: "EditingCauses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Warehouses",
                table: "Warehouses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EditingCauses",
                table: "EditingCauses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Warehouses_WarehouseId",
                table: "AspNetUsers",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WhsProductsDataIn_EditingCauses_EditingCauseId",
                table: "WhsProductsDataIn",
                column: "EditingCauseId",
                principalTable: "EditingCauses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_WhsProductsDataOut_EditingCauses_EditingCauseId",
                table: "WhsProductsDataOut",
                column: "EditingCauseId",
                principalTable: "EditingCauses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}

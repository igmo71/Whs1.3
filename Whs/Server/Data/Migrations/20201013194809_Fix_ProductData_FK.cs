using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Whs.Server.Data.Migrations
{
    public partial class Fix_ProductData_FK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsDataIn_EditingCauses_EditingCauseId",
                table: "ProductsDataIn");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsDataIn_WhsOrdersOut_Документ_Id",
                table: "ProductsDataIn");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsDataOut_EditingCauses_EditingCauseId",
                table: "ProductsDataOut");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsDataOut_WhsOrdersIn_Документ_Id",
                table: "ProductsDataOut");

            migrationBuilder.AlterColumn<Guid>(
                name: "EditingCauseId",
                table: "ProductsDataOut",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "EditingCauseId",
                table: "ProductsDataIn",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsDataIn_EditingCauses_EditingCauseId",
                table: "ProductsDataIn",
                column: "EditingCauseId",
                principalTable: "EditingCauses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsDataIn_WhsOrdersIn_Документ_Id",
                table: "ProductsDataIn",
                column: "Документ_Id",
                principalTable: "WhsOrdersIn",
                principalColumn: "Документ_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsDataOut_EditingCauses_EditingCauseId",
                table: "ProductsDataOut",
                column: "EditingCauseId",
                principalTable: "EditingCauses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsDataOut_WhsOrdersOut_Документ_Id",
                table: "ProductsDataOut",
                column: "Документ_Id",
                principalTable: "WhsOrdersOut",
                principalColumn: "Документ_Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsDataIn_EditingCauses_EditingCauseId",
                table: "ProductsDataIn");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsDataIn_WhsOrdersIn_Документ_Id",
                table: "ProductsDataIn");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsDataOut_EditingCauses_EditingCauseId",
                table: "ProductsDataOut");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsDataOut_WhsOrdersOut_Документ_Id",
                table: "ProductsDataOut");

            migrationBuilder.AlterColumn<Guid>(
                name: "EditingCauseId",
                table: "ProductsDataOut",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "EditingCauseId",
                table: "ProductsDataIn",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsDataIn_EditingCauses_EditingCauseId",
                table: "ProductsDataIn",
                column: "EditingCauseId",
                principalTable: "EditingCauses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsDataIn_WhsOrdersOut_Документ_Id",
                table: "ProductsDataIn",
                column: "Документ_Id",
                principalTable: "WhsOrdersOut",
                principalColumn: "Документ_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsDataOut_EditingCauses_EditingCauseId",
                table: "ProductsDataOut",
                column: "EditingCauseId",
                principalTable: "EditingCauses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsDataOut_WhsOrdersIn_Документ_Id",
                table: "ProductsDataOut",
                column: "Документ_Id",
                principalTable: "WhsOrdersIn",
                principalColumn: "Документ_Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Whs.Server.Data.Migrations
{
    public partial class WhsOrderDataOut_Cascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WhsOrdersDataIn_WhsOrdersIn_Документ_Id",
                table: "WhsOrdersDataIn");

            migrationBuilder.DropForeignKey(
                name: "FK_WhsOrdersDataOut_WhsOrdersOut_Документ_Id",
                table: "WhsOrdersDataOut");

            migrationBuilder.AddForeignKey(
                name: "FK_WhsOrdersDataIn_WhsOrdersIn_Документ_Id",
                table: "WhsOrdersDataIn",
                column: "Документ_Id",
                principalTable: "WhsOrdersIn",
                principalColumn: "Документ_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WhsOrdersDataOut_WhsOrdersOut_Документ_Id",
                table: "WhsOrdersDataOut",
                column: "Документ_Id",
                principalTable: "WhsOrdersOut",
                principalColumn: "Документ_Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WhsOrdersDataIn_WhsOrdersIn_Документ_Id",
                table: "WhsOrdersDataIn");

            migrationBuilder.DropForeignKey(
                name: "FK_WhsOrdersDataOut_WhsOrdersOut_Документ_Id",
                table: "WhsOrdersDataOut");

            migrationBuilder.AddForeignKey(
                name: "FK_WhsOrdersDataIn_WhsOrdersIn_Документ_Id",
                table: "WhsOrdersDataIn",
                column: "Документ_Id",
                principalTable: "WhsOrdersIn",
                principalColumn: "Документ_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WhsOrdersDataOut_WhsOrdersOut_Документ_Id",
                table: "WhsOrdersDataOut",
                column: "Документ_Id",
                principalTable: "WhsOrdersOut",
                principalColumn: "Документ_Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

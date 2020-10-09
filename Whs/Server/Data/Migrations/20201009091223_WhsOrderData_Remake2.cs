using Microsoft.EntityFrameworkCore.Migrations;

namespace Whs.Server.Data.Migrations
{
    public partial class WhsOrderData_Remake2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WhsOrdersDataIn_WhsOrdersIn_WhsOrderInId",
                table: "WhsOrdersDataIn");

            migrationBuilder.DropForeignKey(
                name: "FK_WhsOrdersDataOut_WhsOrdersOut_WhsOrderOutId",
                table: "WhsOrdersDataOut");

            migrationBuilder.DropIndex(
                name: "IX_WhsOrdersDataOut_WhsOrderOutId",
                table: "WhsOrdersDataOut");

            migrationBuilder.DropIndex(
                name: "IX_WhsOrdersDataIn_WhsOrderInId",
                table: "WhsOrdersDataIn");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "WhsOrdersDataOut");

            migrationBuilder.DropColumn(
                name: "WhsOrderOutId",
                table: "WhsOrdersDataOut");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "WhsOrdersDataIn");

            migrationBuilder.DropColumn(
                name: "WhsOrderInId",
                table: "WhsOrdersDataIn");

            migrationBuilder.AddColumn<string>(
                name: "Документ_Id",
                table: "WhsOrdersDataOut",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Статус",
                table: "WhsOrdersDataOut",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Документ_Id",
                table: "WhsOrdersDataIn",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Статус",
                table: "WhsOrdersDataIn",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WhsOrdersDataOut_Документ_Id",
                table: "WhsOrdersDataOut",
                column: "Документ_Id");

            migrationBuilder.CreateIndex(
                name: "IX_WhsOrdersDataIn_Документ_Id",
                table: "WhsOrdersDataIn",
                column: "Документ_Id");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WhsOrdersDataIn_WhsOrdersIn_Документ_Id",
                table: "WhsOrdersDataIn");

            migrationBuilder.DropForeignKey(
                name: "FK_WhsOrdersDataOut_WhsOrdersOut_Документ_Id",
                table: "WhsOrdersDataOut");

            migrationBuilder.DropIndex(
                name: "IX_WhsOrdersDataOut_Документ_Id",
                table: "WhsOrdersDataOut");

            migrationBuilder.DropIndex(
                name: "IX_WhsOrdersDataIn_Документ_Id",
                table: "WhsOrdersDataIn");

            migrationBuilder.DropColumn(
                name: "Документ_Id",
                table: "WhsOrdersDataOut");

            migrationBuilder.DropColumn(
                name: "Статус",
                table: "WhsOrdersDataOut");

            migrationBuilder.DropColumn(
                name: "Документ_Id",
                table: "WhsOrdersDataIn");

            migrationBuilder.DropColumn(
                name: "Статус",
                table: "WhsOrdersDataIn");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "WhsOrdersDataOut",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WhsOrderOutId",
                table: "WhsOrdersDataOut",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "WhsOrdersDataIn",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WhsOrderInId",
                table: "WhsOrdersDataIn",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WhsOrdersDataOut_WhsOrderOutId",
                table: "WhsOrdersDataOut",
                column: "WhsOrderOutId");

            migrationBuilder.CreateIndex(
                name: "IX_WhsOrdersDataIn_WhsOrderInId",
                table: "WhsOrdersDataIn",
                column: "WhsOrderInId");

            migrationBuilder.AddForeignKey(
                name: "FK_WhsOrdersDataIn_WhsOrdersIn_WhsOrderInId",
                table: "WhsOrdersDataIn",
                column: "WhsOrderInId",
                principalTable: "WhsOrdersIn",
                principalColumn: "Документ_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WhsOrdersDataOut_WhsOrdersOut_WhsOrderOutId",
                table: "WhsOrdersDataOut",
                column: "WhsOrderOutId",
                principalTable: "WhsOrdersOut",
                principalColumn: "Документ_Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

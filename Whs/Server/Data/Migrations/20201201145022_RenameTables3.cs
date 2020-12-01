using Microsoft.EntityFrameworkCore.Migrations;

namespace Whs.Server.Data.Migrations
{
    public partial class RenameTables3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MngrOrdersIn_WhsOrdersIn_Документ_Id",
                table: "MngrOrdersIn");

            migrationBuilder.DropForeignKey(
                name: "FK_MngrOrdersOut_WhsOrdersOut_Документ_Id",
                table: "MngrOrdersOut");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MngrOrdersOut",
                table: "MngrOrdersOut");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MngrOrdersIn",
                table: "MngrOrdersIn");

            migrationBuilder.RenameTable(
                name: "MngrOrdersOut",
                newName: "WhsMngrOrdersOut");

            migrationBuilder.RenameTable(
                name: "MngrOrdersIn",
                newName: "WhsMngrOrdersIn");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WhsMngrOrdersOut",
                table: "WhsMngrOrdersOut",
                columns: new[] { "Документ_Id", "Распоряжение_Id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_WhsMngrOrdersIn",
                table: "WhsMngrOrdersIn",
                columns: new[] { "Документ_Id", "Распоряжение_Id" });

            migrationBuilder.AddForeignKey(
                name: "FK_WhsMngrOrdersIn_WhsOrdersIn_Документ_Id",
                table: "WhsMngrOrdersIn",
                column: "Документ_Id",
                principalTable: "WhsOrdersIn",
                principalColumn: "Документ_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WhsMngrOrdersOut_WhsOrdersOut_Документ_Id",
                table: "WhsMngrOrdersOut",
                column: "Документ_Id",
                principalTable: "WhsOrdersOut",
                principalColumn: "Документ_Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WhsMngrOrdersIn_WhsOrdersIn_Документ_Id",
                table: "WhsMngrOrdersIn");

            migrationBuilder.DropForeignKey(
                name: "FK_WhsMngrOrdersOut_WhsOrdersOut_Документ_Id",
                table: "WhsMngrOrdersOut");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WhsMngrOrdersOut",
                table: "WhsMngrOrdersOut");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WhsMngrOrdersIn",
                table: "WhsMngrOrdersIn");

            migrationBuilder.RenameTable(
                name: "WhsMngrOrdersOut",
                newName: "MngrOrdersOut");

            migrationBuilder.RenameTable(
                name: "WhsMngrOrdersIn",
                newName: "MngrOrdersIn");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MngrOrdersOut",
                table: "MngrOrdersOut",
                columns: new[] { "Документ_Id", "Распоряжение_Id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_MngrOrdersIn",
                table: "MngrOrdersIn",
                columns: new[] { "Документ_Id", "Распоряжение_Id" });

            migrationBuilder.AddForeignKey(
                name: "FK_MngrOrdersIn_WhsOrdersIn_Документ_Id",
                table: "MngrOrdersIn",
                column: "Документ_Id",
                principalTable: "WhsOrdersIn",
                principalColumn: "Документ_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MngrOrdersOut_WhsOrdersOut_Документ_Id",
                table: "MngrOrdersOut",
                column: "Документ_Id",
                principalTable: "WhsOrdersOut",
                principalColumn: "Документ_Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Whs.Server.Data.Migrations
{
    public partial class RenameTables2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WhsMngrOrdersOut_WhsOrdersIn_Документ_Id",
                table: "WhsMngrOrdersOut");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WhsMngrOrdersOut",
                table: "WhsMngrOrdersOut");

            migrationBuilder.RenameTable(
                name: "WhsMngrOrdersOut",
                newName: "MngrOrdersIn");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MngrOrdersIn_WhsOrdersIn_Документ_Id",
                table: "MngrOrdersIn");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MngrOrdersIn",
                table: "MngrOrdersIn");

            migrationBuilder.RenameTable(
                name: "MngrOrdersIn",
                newName: "WhsMngrOrdersOut");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WhsMngrOrdersOut",
                table: "WhsMngrOrdersOut",
                columns: new[] { "Документ_Id", "Распоряжение_Id" });

            migrationBuilder.AddForeignKey(
                name: "FK_WhsMngrOrdersOut_WhsOrdersIn_Документ_Id",
                table: "WhsMngrOrdersOut",
                column: "Документ_Id",
                principalTable: "WhsOrdersIn",
                principalColumn: "Документ_Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

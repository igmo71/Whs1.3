using Microsoft.EntityFrameworkCore.Migrations;

namespace Whs.Server.Data.Migrations
{
    public partial class RenameTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MngrOrdersIn_WhsOrdersIn_Документ_Id",
                table: "MngrOrdersIn");

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

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsIn_WhsOrdersIn_Документ_Id",
                table: "ProductsIn");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsOut_WhsOrdersOut_Документ_Id",
                table: "ProductsOut");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductsOut",
                table: "ProductsOut");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductsIn",
                table: "ProductsIn");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductsDataOut",
                table: "ProductsDataOut");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductsDataIn",
                table: "ProductsDataIn");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MngrOrdersIn",
                table: "MngrOrdersIn");

            migrationBuilder.RenameTable(
                name: "ProductsOut",
                newName: "WhsProductsOut");

            migrationBuilder.RenameTable(
                name: "ProductsIn",
                newName: "WhsProductsIn");

            migrationBuilder.RenameTable(
                name: "ProductsDataOut",
                newName: "WhsProductsDataOut");

            migrationBuilder.RenameTable(
                name: "ProductsDataIn",
                newName: "WhsProductsDataIn");

            migrationBuilder.RenameTable(
                name: "MngrOrdersIn",
                newName: "WhsMngrOrdersOut");

            migrationBuilder.RenameIndex(
                name: "IX_ProductsDataOut_Документ_Id",
                table: "WhsProductsDataOut",
                newName: "IX_WhsProductsDataOut_Документ_Id");

            migrationBuilder.RenameIndex(
                name: "IX_ProductsDataOut_EditingCauseId",
                table: "WhsProductsDataOut",
                newName: "IX_WhsProductsDataOut_EditingCauseId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductsDataIn_Документ_Id",
                table: "WhsProductsDataIn",
                newName: "IX_WhsProductsDataIn_Документ_Id");

            migrationBuilder.RenameIndex(
                name: "IX_ProductsDataIn_EditingCauseId",
                table: "WhsProductsDataIn",
                newName: "IX_WhsProductsDataIn_EditingCauseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WhsProductsOut",
                table: "WhsProductsOut",
                columns: new[] { "Документ_Id", "НомерСтроки" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_WhsProductsIn",
                table: "WhsProductsIn",
                columns: new[] { "Документ_Id", "НомерСтроки" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_WhsProductsDataOut",
                table: "WhsProductsDataOut",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WhsProductsDataIn",
                table: "WhsProductsDataIn",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_WhsProductsDataIn_EditingCauses_EditingCauseId",
                table: "WhsProductsDataIn",
                column: "EditingCauseId",
                principalTable: "EditingCauses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_WhsProductsDataIn_WhsOrdersIn_Документ_Id",
                table: "WhsProductsDataIn",
                column: "Документ_Id",
                principalTable: "WhsOrdersIn",
                principalColumn: "Документ_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WhsProductsDataOut_EditingCauses_EditingCauseId",
                table: "WhsProductsDataOut",
                column: "EditingCauseId",
                principalTable: "EditingCauses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_WhsProductsDataOut_WhsOrdersOut_Документ_Id",
                table: "WhsProductsDataOut",
                column: "Документ_Id",
                principalTable: "WhsOrdersOut",
                principalColumn: "Документ_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WhsProductsIn_WhsOrdersIn_Документ_Id",
                table: "WhsProductsIn",
                column: "Документ_Id",
                principalTable: "WhsOrdersIn",
                principalColumn: "Документ_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WhsProductsOut_WhsOrdersOut_Документ_Id",
                table: "WhsProductsOut",
                column: "Документ_Id",
                principalTable: "WhsOrdersOut",
                principalColumn: "Документ_Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WhsMngrOrdersOut_WhsOrdersIn_Документ_Id",
                table: "WhsMngrOrdersOut");

            migrationBuilder.DropForeignKey(
                name: "FK_WhsProductsDataIn_EditingCauses_EditingCauseId",
                table: "WhsProductsDataIn");

            migrationBuilder.DropForeignKey(
                name: "FK_WhsProductsDataIn_WhsOrdersIn_Документ_Id",
                table: "WhsProductsDataIn");

            migrationBuilder.DropForeignKey(
                name: "FK_WhsProductsDataOut_EditingCauses_EditingCauseId",
                table: "WhsProductsDataOut");

            migrationBuilder.DropForeignKey(
                name: "FK_WhsProductsDataOut_WhsOrdersOut_Документ_Id",
                table: "WhsProductsDataOut");

            migrationBuilder.DropForeignKey(
                name: "FK_WhsProductsIn_WhsOrdersIn_Документ_Id",
                table: "WhsProductsIn");

            migrationBuilder.DropForeignKey(
                name: "FK_WhsProductsOut_WhsOrdersOut_Документ_Id",
                table: "WhsProductsOut");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WhsProductsOut",
                table: "WhsProductsOut");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WhsProductsIn",
                table: "WhsProductsIn");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WhsProductsDataOut",
                table: "WhsProductsDataOut");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WhsProductsDataIn",
                table: "WhsProductsDataIn");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WhsMngrOrdersOut",
                table: "WhsMngrOrdersOut");

            migrationBuilder.RenameTable(
                name: "WhsProductsOut",
                newName: "ProductsOut");

            migrationBuilder.RenameTable(
                name: "WhsProductsIn",
                newName: "ProductsIn");

            migrationBuilder.RenameTable(
                name: "WhsProductsDataOut",
                newName: "ProductsDataOut");

            migrationBuilder.RenameTable(
                name: "WhsProductsDataIn",
                newName: "ProductsDataIn");

            migrationBuilder.RenameTable(
                name: "WhsMngrOrdersOut",
                newName: "MngrOrdersIn");

            migrationBuilder.RenameIndex(
                name: "IX_WhsProductsDataOut_Документ_Id",
                table: "ProductsDataOut",
                newName: "IX_ProductsDataOut_Документ_Id");

            migrationBuilder.RenameIndex(
                name: "IX_WhsProductsDataOut_EditingCauseId",
                table: "ProductsDataOut",
                newName: "IX_ProductsDataOut_EditingCauseId");

            migrationBuilder.RenameIndex(
                name: "IX_WhsProductsDataIn_Документ_Id",
                table: "ProductsDataIn",
                newName: "IX_ProductsDataIn_Документ_Id");

            migrationBuilder.RenameIndex(
                name: "IX_WhsProductsDataIn_EditingCauseId",
                table: "ProductsDataIn",
                newName: "IX_ProductsDataIn_EditingCauseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductsOut",
                table: "ProductsOut",
                columns: new[] { "Документ_Id", "НомерСтроки" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductsIn",
                table: "ProductsIn",
                columns: new[] { "Документ_Id", "НомерСтроки" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductsDataOut",
                table: "ProductsDataOut",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductsDataIn",
                table: "ProductsDataIn",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsIn_WhsOrdersIn_Документ_Id",
                table: "ProductsIn",
                column: "Документ_Id",
                principalTable: "WhsOrdersIn",
                principalColumn: "Документ_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsOut_WhsOrdersOut_Документ_Id",
                table: "ProductsOut",
                column: "Документ_Id",
                principalTable: "WhsOrdersOut",
                principalColumn: "Документ_Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

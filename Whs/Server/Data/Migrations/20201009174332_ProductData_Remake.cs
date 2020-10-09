using Microsoft.EntityFrameworkCore.Migrations;

namespace Whs.Server.Data.Migrations
{
    public partial class ProductData_Remake : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsDataIn_ProductsIn_Документ_Id_НомерСтроки",
                table: "ProductsDataIn");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsDataOut_ProductsOut_Документ_Id_НомерСтроки",
                table: "ProductsDataOut");

            migrationBuilder.DropIndex(
                name: "IX_ProductsDataOut_Документ_Id_НомерСтроки",
                table: "ProductsDataOut");

            migrationBuilder.DropIndex(
                name: "IX_ProductsDataIn_Документ_Id_НомерСтроки",
                table: "ProductsDataIn");

            migrationBuilder.AlterColumn<string>(
                name: "Документ_Id",
                table: "ProductsDataOut",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Артикул",
                table: "ProductsDataOut",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Вес",
                table: "ProductsDataOut",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "Документ_Name",
                table: "ProductsDataOut",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "КоличествоПлан",
                table: "ProductsDataOut",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "КоличествоФакт",
                table: "ProductsDataOut",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "Номенклатура_Id",
                table: "ProductsDataOut",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Номенклатура_Name",
                table: "ProductsDataOut",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Упаковка_Id",
                table: "ProductsDataOut",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Упаковка_Name",
                table: "ProductsDataOut",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Документ_Id",
                table: "ProductsDataIn",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Артикул",
                table: "ProductsDataIn",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Вес",
                table: "ProductsDataIn",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "Документ_Name",
                table: "ProductsDataIn",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "КоличествоПлан",
                table: "ProductsDataIn",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "КоличествоФакт",
                table: "ProductsDataIn",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "Номенклатура_Id",
                table: "ProductsDataIn",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Номенклатура_Name",
                table: "ProductsDataIn",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Упаковка_Id",
                table: "ProductsDataIn",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Упаковка_Name",
                table: "ProductsDataIn",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Артикул",
                table: "ProductsDataOut");

            migrationBuilder.DropColumn(
                name: "Вес",
                table: "ProductsDataOut");

            migrationBuilder.DropColumn(
                name: "Документ_Name",
                table: "ProductsDataOut");

            migrationBuilder.DropColumn(
                name: "КоличествоПлан",
                table: "ProductsDataOut");

            migrationBuilder.DropColumn(
                name: "КоличествоФакт",
                table: "ProductsDataOut");

            migrationBuilder.DropColumn(
                name: "Номенклатура_Id",
                table: "ProductsDataOut");

            migrationBuilder.DropColumn(
                name: "Номенклатура_Name",
                table: "ProductsDataOut");

            migrationBuilder.DropColumn(
                name: "Упаковка_Id",
                table: "ProductsDataOut");

            migrationBuilder.DropColumn(
                name: "Упаковка_Name",
                table: "ProductsDataOut");

            migrationBuilder.DropColumn(
                name: "Артикул",
                table: "ProductsDataIn");

            migrationBuilder.DropColumn(
                name: "Вес",
                table: "ProductsDataIn");

            migrationBuilder.DropColumn(
                name: "Документ_Name",
                table: "ProductsDataIn");

            migrationBuilder.DropColumn(
                name: "КоличествоПлан",
                table: "ProductsDataIn");

            migrationBuilder.DropColumn(
                name: "КоличествоФакт",
                table: "ProductsDataIn");

            migrationBuilder.DropColumn(
                name: "Номенклатура_Id",
                table: "ProductsDataIn");

            migrationBuilder.DropColumn(
                name: "Номенклатура_Name",
                table: "ProductsDataIn");

            migrationBuilder.DropColumn(
                name: "Упаковка_Id",
                table: "ProductsDataIn");

            migrationBuilder.DropColumn(
                name: "Упаковка_Name",
                table: "ProductsDataIn");

            migrationBuilder.AlterColumn<string>(
                name: "Документ_Id",
                table: "ProductsDataOut",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Документ_Id",
                table: "ProductsDataIn",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductsDataOut_Документ_Id_НомерСтроки",
                table: "ProductsDataOut",
                columns: new[] { "Документ_Id", "НомерСтроки" },
                unique: true,
                filter: "[Документ_Id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsDataIn_Документ_Id_НомерСтроки",
                table: "ProductsDataIn",
                columns: new[] { "Документ_Id", "НомерСтроки" },
                unique: true,
                filter: "[Документ_Id] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsDataIn_ProductsIn_Документ_Id_НомерСтроки",
                table: "ProductsDataIn",
                columns: new[] { "Документ_Id", "НомерСтроки" },
                principalTable: "ProductsIn",
                principalColumns: new[] { "Документ_Id", "НомерСтроки" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsDataOut_ProductsOut_Документ_Id_НомерСтроки",
                table: "ProductsDataOut",
                columns: new[] { "Документ_Id", "НомерСтроки" },
                principalTable: "ProductsOut",
                principalColumns: new[] { "Документ_Id", "НомерСтроки" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}

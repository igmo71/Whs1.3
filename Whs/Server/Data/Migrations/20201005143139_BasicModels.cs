using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Whs.Server.Data.Migrations
{
    public partial class BasicModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WhsOrdersIn",
                columns: table => new
                {
                    Документ_Id = table.Column<string>(nullable: false),
                    Документ_Name = table.Column<string>(nullable: true),
                    Номер = table.Column<string>(nullable: true),
                    Дата = table.Column<DateTime>(nullable: false),
                    Проведен = table.Column<bool>(nullable: false),
                    Статус = table.Column<string>(nullable: true),
                    КоличествоСтрок = table.Column<int>(nullable: false),
                    Вес = table.Column<float>(nullable: false),
                    СрокВыполнения = table.Column<DateTime>(nullable: false),
                    ТипОчереди = table.Column<string>(nullable: true),
                    ВесовойКоэффициент = table.Column<int>(nullable: false),
                    Комментарий = table.Column<string>(nullable: true),
                    ЭтоПеремещение = table.Column<bool>(nullable: false),
                    Склад_Id = table.Column<string>(nullable: true),
                    Склад_Name = table.Column<string>(nullable: true),
                    Ответственный_Id = table.Column<string>(nullable: true),
                    Ответственный_Name = table.Column<string>(nullable: true),
                    ОтправительПолучатель_Id = table.Column<string>(nullable: true),
                    ОтправительПолучатель_Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WhsOrdersIn", x => x.Документ_Id);
                });

            migrationBuilder.CreateTable(
                name: "WhsOrdersOut",
                columns: table => new
                {
                    Документ_Id = table.Column<string>(nullable: false),
                    Документ_Name = table.Column<string>(nullable: true),
                    Номер = table.Column<string>(nullable: true),
                    Дата = table.Column<DateTime>(nullable: false),
                    Проведен = table.Column<bool>(nullable: false),
                    Статус = table.Column<string>(nullable: true),
                    КоличествоСтрок = table.Column<int>(nullable: false),
                    Вес = table.Column<float>(nullable: false),
                    СрокВыполнения = table.Column<DateTime>(nullable: false),
                    ТипОчереди = table.Column<string>(nullable: true),
                    ВесовойКоэффициент = table.Column<int>(nullable: false),
                    Комментарий = table.Column<string>(nullable: true),
                    ЭтоПеремещение = table.Column<bool>(nullable: false),
                    Склад_Id = table.Column<string>(nullable: true),
                    Склад_Name = table.Column<string>(nullable: true),
                    Ответственный_Id = table.Column<string>(nullable: true),
                    Ответственный_Name = table.Column<string>(nullable: true),
                    ОтправительПолучатель_Id = table.Column<string>(nullable: true),
                    ОтправительПолучатель_Name = table.Column<string>(nullable: true),
                    НомерОчереди = table.Column<string>(nullable: true),
                    НаправлениеДоставки_Id = table.Column<string>(nullable: true),
                    НаправлениеДоставки_Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WhsOrdersOut", x => x.Документ_Id);
                });

            migrationBuilder.CreateTable(
                name: "MngrOrdersIn",
                columns: table => new
                {
                    Документ_Id = table.Column<string>(nullable: false),
                    Распоряжение_Id = table.Column<string>(nullable: false),
                    Документ_Name = table.Column<string>(nullable: true),
                    Распоряжение_Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MngrOrdersIn", x => new { x.Документ_Id, x.Распоряжение_Id });
                    table.ForeignKey(
                        name: "FK_MngrOrdersIn_WhsOrdersIn_Документ_Id",
                        column: x => x.Документ_Id,
                        principalTable: "WhsOrdersIn",
                        principalColumn: "Документ_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductsIn",
                columns: table => new
                {
                    Документ_Id = table.Column<string>(nullable: false),
                    НомерСтроки = table.Column<int>(nullable: false),
                    Документ_Name = table.Column<string>(nullable: true),
                    Номенклатура_Id = table.Column<string>(nullable: true),
                    Номенклатура_Name = table.Column<string>(nullable: true),
                    Артикул = table.Column<string>(nullable: true),
                    КоличествоФакт = table.Column<float>(nullable: false),
                    КоличествоПлан = table.Column<float>(nullable: false),
                    Вес = table.Column<float>(nullable: false),
                    Упаковка_Id = table.Column<string>(nullable: true),
                    Упаковка_Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsIn", x => new { x.Документ_Id, x.НомерСтроки });
                    table.ForeignKey(
                        name: "FK_ProductsIn_WhsOrdersIn_Документ_Id",
                        column: x => x.Документ_Id,
                        principalTable: "WhsOrdersIn",
                        principalColumn: "Документ_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MngrOrdersOut",
                columns: table => new
                {
                    Документ_Id = table.Column<string>(nullable: false),
                    Распоряжение_Id = table.Column<string>(nullable: false),
                    Документ_Name = table.Column<string>(nullable: true),
                    Распоряжение_Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MngrOrdersOut", x => new { x.Документ_Id, x.Распоряжение_Id });
                    table.ForeignKey(
                        name: "FK_MngrOrdersOut_WhsOrdersOut_Документ_Id",
                        column: x => x.Документ_Id,
                        principalTable: "WhsOrdersOut",
                        principalColumn: "Документ_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductsOut",
                columns: table => new
                {
                    Документ_Id = table.Column<string>(nullable: false),
                    НомерСтроки = table.Column<int>(nullable: false),
                    Документ_Name = table.Column<string>(nullable: true),
                    Номенклатура_Id = table.Column<string>(nullable: true),
                    Номенклатура_Name = table.Column<string>(nullable: true),
                    Артикул = table.Column<string>(nullable: true),
                    КоличествоФакт = table.Column<float>(nullable: false),
                    КоличествоПлан = table.Column<float>(nullable: false),
                    Вес = table.Column<float>(nullable: false),
                    Упаковка_Id = table.Column<string>(nullable: true),
                    Упаковка_Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsOut", x => new { x.Документ_Id, x.НомерСтроки });
                    table.ForeignKey(
                        name: "FK_ProductsOut_WhsOrdersOut_Документ_Id",
                        column: x => x.Документ_Id,
                        principalTable: "WhsOrdersOut",
                        principalColumn: "Документ_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MngOrdersIn_ДокументIdРаспоряжениеId",
                table: "MngrOrdersIn",
                columns: new[] { "Документ_Id", "Распоряжение_Id" });

            migrationBuilder.CreateIndex(
                name: "IX_MngOrdersOut_ДокументIdРаспоряжениеId",
                table: "MngrOrdersOut",
                columns: new[] { "Документ_Id", "Распоряжение_Id" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductsIn_ДокументIdНомерСтроки",
                table: "ProductsIn",
                columns: new[] { "Документ_Id", "НомерСтроки" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductsOut_ДокументIdНомерСтроки",
                table: "ProductsOut",
                columns: new[] { "Документ_Id", "НомерСтроки" });

            migrationBuilder.CreateIndex(
                name: "IX_WhsOrdersIn_ДокументId",
                table: "WhsOrdersIn",
                column: "Документ_Id");

            migrationBuilder.CreateIndex(
                name: "IX_WhsOrdersOut_ДокументId",
                table: "WhsOrdersOut",
                column: "Документ_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MngrOrdersIn");

            migrationBuilder.DropTable(
                name: "MngrOrdersOut");

            migrationBuilder.DropTable(
                name: "ProductsIn");

            migrationBuilder.DropTable(
                name: "ProductsOut");

            migrationBuilder.DropTable(
                name: "WhsOrdersIn");

            migrationBuilder.DropTable(
                name: "WhsOrdersOut");
        }
    }
}

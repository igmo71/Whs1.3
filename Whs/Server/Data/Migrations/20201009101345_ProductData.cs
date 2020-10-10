using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Whs.Server.Data.Migrations
{
    public partial class ProductData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EditingCauses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EditingCauses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductsDataIn",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Документ_Id = table.Column<string>(nullable: true),
                    НомерСтроки = table.Column<int>(nullable: false),
                    EditingCauseId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsDataIn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductsDataIn_EditingCauses_EditingCauseId",
                        column: x => x.EditingCauseId,
                        principalTable: "EditingCauses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsDataIn_ProductsIn_Документ_Id_НомерСтроки",
                        columns: x => new { x.Документ_Id, x.НомерСтроки },
                        principalTable: "ProductsIn",
                        principalColumns: new[] { "Документ_Id", "НомерСтроки" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductsDataOut",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Документ_Id = table.Column<string>(nullable: true),
                    НомерСтроки = table.Column<int>(nullable: false),
                    EditingCauseId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsDataOut", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductsDataOut_EditingCauses_EditingCauseId",
                        column: x => x.EditingCauseId,
                        principalTable: "EditingCauses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsDataOut_ProductsOut_Документ_Id_НомерСтроки",
                        columns: x => new { x.Документ_Id, x.НомерСтроки },
                        principalTable: "ProductsOut",
                        principalColumns: new[] { "Документ_Id", "НомерСтроки" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductsDataIn_EditingCauseId",
                table: "ProductsDataIn",
                column: "EditingCauseId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsDataIn_Документ_Id_НомерСтроки",
                table: "ProductsDataIn",
                columns: new[] { "Документ_Id", "НомерСтроки" },
                unique: true,
                filter: "[Документ_Id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsDataOut_EditingCauseId",
                table: "ProductsDataOut",
                column: "EditingCauseId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsDataOut_Документ_Id_НомерСтроки",
                table: "ProductsDataOut",
                columns: new[] { "Документ_Id", "НомерСтроки" },
                unique: true,
                filter: "[Документ_Id] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductsDataIn");

            migrationBuilder.DropTable(
                name: "ProductsDataOut");

            migrationBuilder.DropTable(
                name: "EditingCauses");
        }
    }
}

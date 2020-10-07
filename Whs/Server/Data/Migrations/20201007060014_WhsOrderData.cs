using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Whs.Server.Data.Migrations
{
    public partial class WhsOrderData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WhsOrdersDataIn",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    OldStatus = table.Column<string>(nullable: true),
                    NewStatus = table.Column<string>(nullable: true),
                    StatusSwitchDate = table.Column<DateTime>(nullable: false),
                    WhsOrderInId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WhsOrdersDataIn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WhsOrdersDataIn_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WhsOrdersDataIn_WhsOrdersIn_WhsOrderInId",
                        column: x => x.WhsOrderInId,
                        principalTable: "WhsOrdersIn",
                        principalColumn: "Документ_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WhsOrdersDataOut",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    OldStatus = table.Column<string>(nullable: true),
                    NewStatus = table.Column<string>(nullable: true),
                    StatusSwitchDate = table.Column<DateTime>(nullable: false),
                    WhsOrderOutId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WhsOrdersDataOut", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WhsOrdersDataOut_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WhsOrdersDataOut_WhsOrdersOut_WhsOrderOutId",
                        column: x => x.WhsOrderOutId,
                        principalTable: "WhsOrdersOut",
                        principalColumn: "Документ_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WhsOrdersDataIn_ApplicationUserId",
                table: "WhsOrdersDataIn",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WhsOrdersDataIn_WhsOrderInId",
                table: "WhsOrdersDataIn",
                column: "WhsOrderInId");

            migrationBuilder.CreateIndex(
                name: "IX_WhsOrdersDataOut_ApplicationUserId",
                table: "WhsOrdersDataOut",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WhsOrdersDataOut_WhsOrderOutId",
                table: "WhsOrdersDataOut",
                column: "WhsOrderOutId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WhsOrdersDataIn");

            migrationBuilder.DropTable(
                name: "WhsOrdersDataOut");
        }
    }
}

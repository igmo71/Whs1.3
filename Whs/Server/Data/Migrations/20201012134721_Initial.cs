using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Whs.Server.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

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
                name: "Warehouses",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.Id);
                });

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
                    НаправлениеДоставки_Name = table.Column<string>(nullable: true),
                    Оплачено = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WhsOrdersOut", x => x.Документ_Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FullName = table.Column<string>(nullable: true),
                    WarehouseId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
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
                name: "ProductsDataOut",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Документ_Id = table.Column<string>(nullable: true),
                    НомерСтроки = table.Column<int>(nullable: false),
                    Документ_Name = table.Column<string>(nullable: true),
                    Номенклатура_Id = table.Column<string>(nullable: true),
                    Номенклатура_Name = table.Column<string>(nullable: true),
                    Артикул = table.Column<string>(nullable: true),
                    КоличествоФакт = table.Column<float>(nullable: false),
                    КоличествоПлан = table.Column<float>(nullable: false),
                    Вес = table.Column<float>(nullable: false),
                    Упаковка_Id = table.Column<string>(nullable: true),
                    Упаковка_Name = table.Column<string>(nullable: true),
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
                        name: "FK_ProductsDataOut_WhsOrdersIn_Документ_Id",
                        column: x => x.Документ_Id,
                        principalTable: "WhsOrdersIn",
                        principalColumn: "Документ_Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "ProductsDataIn",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Документ_Id = table.Column<string>(nullable: true),
                    НомерСтроки = table.Column<int>(nullable: false),
                    Документ_Name = table.Column<string>(nullable: true),
                    Номенклатура_Id = table.Column<string>(nullable: true),
                    Номенклатура_Name = table.Column<string>(nullable: true),
                    Артикул = table.Column<string>(nullable: true),
                    КоличествоФакт = table.Column<float>(nullable: false),
                    КоличествоПлан = table.Column<float>(nullable: false),
                    Вес = table.Column<float>(nullable: false),
                    Упаковка_Id = table.Column<string>(nullable: true),
                    Упаковка_Name = table.Column<string>(nullable: true),
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
                        name: "FK_ProductsDataIn_WhsOrdersOut_Документ_Id",
                        column: x => x.Документ_Id,
                        principalTable: "WhsOrdersOut",
                        principalColumn: "Документ_Id",
                        onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WhsOrdersDataIn",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Документ_Id = table.Column<string>(nullable: true),
                    Статус = table.Column<string>(nullable: true),
                    DateTime = table.Column<DateTime>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: true)
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
                        name: "FK_WhsOrdersDataIn_WhsOrdersIn_Документ_Id",
                        column: x => x.Документ_Id,
                        principalTable: "WhsOrdersIn",
                        principalColumn: "Документ_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WhsOrdersDataOut",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Документ_Id = table.Column<string>(nullable: true),
                    Статус = table.Column<string>(nullable: true),
                    DateTime = table.Column<DateTime>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: true)
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
                        name: "FK_WhsOrdersDataOut_WhsOrdersOut_Документ_Id",
                        column: x => x.Документ_Id,
                        principalTable: "WhsOrdersOut",
                        principalColumn: "Документ_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_WarehouseId",
                table: "AspNetUsers",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_MngOrdersIn_ДокументIdРаспоряжениеId",
                table: "MngrOrdersIn",
                columns: new[] { "Документ_Id", "Распоряжение_Id" });

            migrationBuilder.CreateIndex(
                name: "IX_MngOrdersOut_ДокументIdРаспоряжениеId",
                table: "MngrOrdersOut",
                columns: new[] { "Документ_Id", "Распоряжение_Id" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductsDataIn_EditingCauseId",
                table: "ProductsDataIn",
                column: "EditingCauseId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsDataIn_Документ_Id",
                table: "ProductsDataIn",
                column: "Документ_Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsDataOut_EditingCauseId",
                table: "ProductsDataOut",
                column: "EditingCauseId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsDataOut_Документ_Id",
                table: "ProductsDataOut",
                column: "Документ_Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsIn_ДокументIdНомерСтроки",
                table: "ProductsIn",
                columns: new[] { "Документ_Id", "НомерСтроки" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductsOut_ДокументIdНомерСтроки",
                table: "ProductsOut",
                columns: new[] { "Документ_Id", "НомерСтроки" });

            migrationBuilder.CreateIndex(
                name: "IX_WhsOrdersDataIn_ApplicationUserId",
                table: "WhsOrdersDataIn",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WhsOrdersDataIn_Документ_Id",
                table: "WhsOrdersDataIn",
                column: "Документ_Id");

            migrationBuilder.CreateIndex(
                name: "IX_WhsOrdersDataOut_ApplicationUserId",
                table: "WhsOrdersDataOut",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WhsOrdersDataOut_Документ_Id",
                table: "WhsOrdersDataOut",
                column: "Документ_Id");

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
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "MngrOrdersIn");

            migrationBuilder.DropTable(
                name: "MngrOrdersOut");

            migrationBuilder.DropTable(
                name: "ProductsDataIn");

            migrationBuilder.DropTable(
                name: "ProductsDataOut");

            migrationBuilder.DropTable(
                name: "ProductsIn");

            migrationBuilder.DropTable(
                name: "ProductsOut");

            migrationBuilder.DropTable(
                name: "WhsOrdersDataIn");

            migrationBuilder.DropTable(
                name: "WhsOrdersDataOut");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "EditingCauses");

            migrationBuilder.DropTable(
                name: "WhsOrdersIn");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "WhsOrdersOut");

            migrationBuilder.DropTable(
                name: "Warehouses");
        }
    }
}

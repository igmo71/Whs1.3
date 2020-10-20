using Microsoft.EntityFrameworkCore.Migrations;

namespace Whs.Server.Data.Migrations
{
    public partial class НаправлениеДоставкиРодитель : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "НаправлениеДоставкиРодитель_Id",
                table: "WhsOrdersOut",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "НаправлениеДоставкиРодитель_Name",
                table: "WhsOrdersOut",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "НаправлениеДоставкиРодитель_Id",
                table: "WhsOrdersOut");

            migrationBuilder.DropColumn(
                name: "НаправлениеДоставкиРодитель_Name",
                table: "WhsOrdersOut");
        }
    }
}

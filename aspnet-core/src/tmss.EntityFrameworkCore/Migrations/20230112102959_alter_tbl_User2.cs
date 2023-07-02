using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class alter_tbl_User2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "VenderAccountId",
                table: "AbpUsers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VenderAccountId",
                table: "AbpUsers");
        }
    }
}

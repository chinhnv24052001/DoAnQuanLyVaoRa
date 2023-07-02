using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class update_table_AioAsset_AioEmployees : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Status",
                table: "AioEmployees",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Status",
                table: "AioAsset",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "AioEmployees");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "AioAsset");
        }
    }
}

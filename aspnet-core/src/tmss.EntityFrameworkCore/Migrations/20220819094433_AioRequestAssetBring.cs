using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class AioRequestAssetBring : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Department",
                table: "AioRequestAssetBring");

            migrationBuilder.DropColumn(
                name: "VenderId",
                table: "AioEmployees");

            migrationBuilder.AddColumn<long>(
                name: "VenderId",
                table: "AioRequestAssetBring",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VenderId",
                table: "AioRequestAssetBring");

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "AioRequestAssetBring",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "VenderId",
                table: "AioEmployees",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}

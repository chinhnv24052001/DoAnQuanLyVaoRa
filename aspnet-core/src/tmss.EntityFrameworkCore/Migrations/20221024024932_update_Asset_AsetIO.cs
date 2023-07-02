using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class update_Asset_AsetIO : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TagCode",
                table: "MstAsset");

            migrationBuilder.AddColumn<string>(
                name: "TagCode",
                table: "AioRequestAsset",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TagCode",
                table: "AioRequestAsset");

            migrationBuilder.AddColumn<string>(
                name: "TagCode",
                table: "MstAsset",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

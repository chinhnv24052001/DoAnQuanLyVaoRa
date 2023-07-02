using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class alter_tbl_AioRequestAsset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "AioRequestAsset",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "AioRequestAsset");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class update_table_AssetReaquestBring_004 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Adm",
                table: "AioRequestAssetBring",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Manager",
                table: "AioRequestAssetBring",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Adm",
                table: "AioRequestAssetBring");

            migrationBuilder.DropColumn(
                name: "Manager",
                table: "AioRequestAssetBring");
        }
    }
}

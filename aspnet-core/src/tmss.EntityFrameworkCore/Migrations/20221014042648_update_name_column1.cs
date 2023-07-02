using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class update_name_column1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssetInOutId",
                table: "AioRequestAssetInOut");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AssetInOutId",
                table: "AioRequestAssetInOut",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}

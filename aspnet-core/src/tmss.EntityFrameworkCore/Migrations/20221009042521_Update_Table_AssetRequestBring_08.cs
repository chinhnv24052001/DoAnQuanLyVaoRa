using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class Update_Table_AssetRequestBring_08 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TemManagerApproval",
                table: "AioRequestAssetBring",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TemManagerApproval",
                table: "AioRequestAssetBring");
        }
    }
}

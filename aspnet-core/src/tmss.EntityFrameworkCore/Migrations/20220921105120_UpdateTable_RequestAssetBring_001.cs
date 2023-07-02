using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class UpdateTable_RequestAssetBring_001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ManageApprovalId",
                table: "AioRequestAssetBring");

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "AioRequestAssetBring",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AioRequestAssetBring");

            migrationBuilder.AddColumn<long>(
                name: "ManageApprovalId",
                table: "AioRequestAssetBring",
                type: "bigint",
                nullable: true);
        }
    }
}

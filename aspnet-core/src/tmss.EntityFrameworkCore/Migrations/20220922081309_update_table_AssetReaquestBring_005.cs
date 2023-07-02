using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class update_table_AssetReaquestBring_005 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Manager",
                table: "AioRequestAssetBring",
                newName: "ManageApproval");

            migrationBuilder.RenameColumn(
                name: "Manager",
                table: "AbpUsers",
                newName: "ManageApproval");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ManageApproval",
                table: "AioRequestAssetBring",
                newName: "Manager");

            migrationBuilder.RenameColumn(
                name: "ManageApproval",
                table: "AbpUsers",
                newName: "Manager");
        }
    }
}

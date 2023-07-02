using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class Update_Table_RequestAssetBring_Add_Column_Status : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "AioRequestAssetBring",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddColumn<long>(
                name: "AdminApprovalId",
                table: "AioRequestAssetBring",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ManageApprovalId",
                table: "AioRequestAssetBring",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequestCode",
                table: "AioRequestAssetBring",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "AioRequestAssetBring",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminApprovalId",
                table: "AioRequestAssetBring");

            migrationBuilder.DropColumn(
                name: "ManageApprovalId",
                table: "AioRequestAssetBring");

            migrationBuilder.DropColumn(
                name: "RequestCode",
                table: "AioRequestAssetBring");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "AioRequestAssetBring");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "AioRequestAssetBring",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);
        }
    }
}

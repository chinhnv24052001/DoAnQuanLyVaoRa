using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class Update_table_AioRequestAsset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AmRequestAssetBring",
                table: "AmRequestAssetBring");

            migrationBuilder.RenameTable(
                name: "AmRequestAssetBring",
                newName: "AioRequestAssetBring");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateAdminApproval",
                table: "AioRequestAssetBring",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateManageApproval",
                table: "AioRequestAssetBring",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRequest",
                table: "AioRequestAssetBring",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "AioRequestAssetBring",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AioRequestAssetBring",
                table: "AioRequestAssetBring",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AioRequestAssetBring",
                table: "AioRequestAssetBring");

            migrationBuilder.DropColumn(
                name: "DateAdminApproval",
                table: "AioRequestAssetBring");

            migrationBuilder.DropColumn(
                name: "DateManageApproval",
                table: "AioRequestAssetBring");

            migrationBuilder.DropColumn(
                name: "DateRequest",
                table: "AioRequestAssetBring");

            migrationBuilder.DropColumn(
                name: "Department",
                table: "AioRequestAssetBring");

            migrationBuilder.RenameTable(
                name: "AioRequestAssetBring",
                newName: "AmRequestAssetBring");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AmRequestAssetBring",
                table: "AmRequestAssetBring",
                column: "Id");
        }
    }
}

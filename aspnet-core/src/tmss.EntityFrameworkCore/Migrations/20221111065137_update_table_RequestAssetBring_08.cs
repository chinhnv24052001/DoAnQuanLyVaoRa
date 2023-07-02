using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class update_table_RequestAssetBring_08 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkingDateEnd",
                table: "MstEmployeesLearnedSafety");

            migrationBuilder.DropColumn(
                name: "WorkingDateStart",
                table: "MstEmployeesLearnedSafety");

            migrationBuilder.AddColumn<string>(
                name: "LiveMonitorDepartment",
                table: "AioRequest",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LiveMonitorName",
                table: "AioRequest",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LiveMonitorPhoneNumber",
                table: "AioRequest",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LiveMonitorDepartment",
                table: "AioRequest");

            migrationBuilder.DropColumn(
                name: "LiveMonitorName",
                table: "AioRequest");

            migrationBuilder.DropColumn(
                name: "LiveMonitorPhoneNumber",
                table: "AioRequest");

            migrationBuilder.AddColumn<DateTime>(
                name: "WorkingDateEnd",
                table: "MstEmployeesLearnedSafety",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "WorkingDateStart",
                table: "MstEmployeesLearnedSafety",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}

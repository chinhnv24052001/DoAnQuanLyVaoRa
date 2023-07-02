using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class update_table_EmployeedSafety_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PersonInCharge",
                table: "MstEmployeesLearnedSafety",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "WorkingDateStart",
                table: "MstEmployeesLearnedSafety",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "WorkingDateTo",
                table: "MstEmployeesLearnedSafety",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PersonInCharge",
                table: "MstEmployeesLearnedSafety");

            migrationBuilder.DropColumn(
                name: "WorkingDateStart",
                table: "MstEmployeesLearnedSafety");

            migrationBuilder.DropColumn(
                name: "WorkingDateTo",
                table: "MstEmployeesLearnedSafety");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class UPdate_table_CourceSafety02_table_EmployeesLearndedSafety02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourceCode",
                table: "MstTemEmployeesLearnedSafety");

            migrationBuilder.DropColumn(
                name: "CourceCode",
                table: "MstEmployeesLearnedSafety");

            migrationBuilder.DropColumn(
                name: "CourceCode",
                table: "MstCourceSafety");

            migrationBuilder.AddColumn<long>(
                name: "CourceId",
                table: "MstTemEmployeesLearnedSafety",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "CourceId",
                table: "MstEmployeesLearnedSafety",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MstCourceSafety",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourceId",
                table: "MstTemEmployeesLearnedSafety");

            migrationBuilder.DropColumn(
                name: "CourceId",
                table: "MstEmployeesLearnedSafety");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "MstCourceSafety");

            migrationBuilder.AddColumn<string>(
                name: "CourceCode",
                table: "MstTemEmployeesLearnedSafety",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CourceCode",
                table: "MstEmployeesLearnedSafety",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CourceCode",
                table: "MstCourceSafety",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }
    }
}

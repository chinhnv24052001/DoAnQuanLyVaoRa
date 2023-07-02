using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class update_table_employessLearned_safety : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "MstEmployeesLearnedSafety",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Gender",
                table: "MstEmployeesLearnedSafety",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "MstEmployeesLearnedSafety",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "MstEmployeesLearnedSafety");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "MstEmployeesLearnedSafety");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "MstEmployeesLearnedSafety");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class update_table_MstCourceSafety_MstEmployeesLearnedSafety : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EffectiveDate",
                table: "MstEmployeesLearnedSafetys",
                newName: "IdentityCard");

            migrationBuilder.AddColumn<string>(
                name: "CourceCode",
                table: "MstEmployeesLearnedSafetys",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CourceCode",
                table: "MstCourceSafety",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourceCode",
                table: "MstEmployeesLearnedSafetys");

            migrationBuilder.DropColumn(
                name: "CourceCode",
                table: "MstCourceSafety");

            migrationBuilder.RenameColumn(
                name: "IdentityCard",
                table: "MstEmployeesLearnedSafetys",
                newName: "EffectiveDate");
        }
    }
}

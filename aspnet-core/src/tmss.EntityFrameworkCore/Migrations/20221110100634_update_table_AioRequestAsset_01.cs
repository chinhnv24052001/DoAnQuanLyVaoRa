using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class update_table_AioRequestAsset_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WorkingDateTo",
                table: "MstEmployeesLearnedSafety",
                newName: "WorkingDateEnd");

            migrationBuilder.AlterColumn<string>(
                name: "SeriNumber",
                table: "AioRequestAsset",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WorkingDateEnd",
                table: "MstEmployeesLearnedSafety",
                newName: "WorkingDateTo");

            migrationBuilder.AlterColumn<string>(
                name: "SeriNumber",
                table: "AioRequestAsset",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}

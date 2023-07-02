using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class Updatetableemployees : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Vender",
                table: "MstEmployeesLearnedSafety",
                newName: "VenderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VenderId",
                table: "MstEmployeesLearnedSafety",
                newName: "Vender");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class Update_Table_AssetManament : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Approved",
                table: "AssetManament",
                newName: "DepartmentApproved");

            migrationBuilder.AddColumn<bool>(
                name: "AdminApproved",
                table: "AssetManament",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminApproved",
                table: "AssetManament");

            migrationBuilder.RenameColumn(
                name: "DepartmentApproved",
                table: "AssetManament",
                newName: "Approved");
        }
    }
}

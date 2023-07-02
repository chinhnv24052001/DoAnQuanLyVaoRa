using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class update_table_MstStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "MstStatus",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "MstStatus",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Key",
                table: "MstStatus");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "MstStatus",
                newName: "Status");
        }
    }
}

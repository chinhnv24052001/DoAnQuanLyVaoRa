using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class alter_tbl_MstVenderAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VenderAccountName",
                table: "MstVenderAccount",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VenderAccountName",
                table: "MstVenderAccount");
        }
    }
}

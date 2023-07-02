using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class alter_tbl_user : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ManageApproval",
                table: "AbpUsers",
                newName: "AccountManager");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccountManager",
                table: "AbpUsers",
                newName: "ManageApproval");
        }
    }
}

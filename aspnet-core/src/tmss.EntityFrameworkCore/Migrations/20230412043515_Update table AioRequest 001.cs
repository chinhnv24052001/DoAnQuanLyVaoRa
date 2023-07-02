using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class UpdatetableAioRequest001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdmApprovalMessage",
                table: "AioRequest",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ManageApprovalMessage",
                table: "AioRequest",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdmApprovalMessage",
                table: "AioRequest");

            migrationBuilder.DropColumn(
                name: "ManageApprovalMessage",
                table: "AioRequest");
        }
    }
}

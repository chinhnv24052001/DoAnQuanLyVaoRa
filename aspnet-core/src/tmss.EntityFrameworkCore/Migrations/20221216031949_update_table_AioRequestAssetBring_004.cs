using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class update_table_AioRequestAssetBring_004 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PersonInChangeOfSubPhone",
                table: "AioRequest",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PersonInChargeOfSubName",
                table: "AioRequest",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PersonInChangeOfSubPhone",
                table: "AioRequest");

            migrationBuilder.DropColumn(
                name: "PersonInChargeOfSubName",
                table: "AioRequest");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class update_table_AioRequestActionLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "RequestId",
                table: "AioRequestActionLog",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestId",
                table: "AioRequestActionLog");
        }
    }
}

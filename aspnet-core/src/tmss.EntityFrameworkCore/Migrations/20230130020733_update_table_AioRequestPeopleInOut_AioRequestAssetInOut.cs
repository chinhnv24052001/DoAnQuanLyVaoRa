using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class update_table_AioRequestPeopleInOut_AioRequestAssetInOut : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "NoteId",
                table: "AioRequestPeopleInOut",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "NoteId",
                table: "AioRequestAssetInOut",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoteId",
                table: "AioRequestPeopleInOut");

            migrationBuilder.DropColumn(
                name: "NoteId",
                table: "AioRequestAssetInOut");
        }
    }
}

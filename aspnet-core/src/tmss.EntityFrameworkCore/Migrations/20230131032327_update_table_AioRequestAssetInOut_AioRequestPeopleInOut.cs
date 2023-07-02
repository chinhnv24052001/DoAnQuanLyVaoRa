using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class update_table_AioRequestAssetInOut_AioRequestPeopleInOut : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NoteId",
                table: "AioRequestPeopleInOut",
                newName: "NoteOutId");

            migrationBuilder.RenameColumn(
                name: "NoteId",
                table: "AioRequestAssetInOut",
                newName: "NoteOutId");

            migrationBuilder.AddColumn<long>(
                name: "NoteInId",
                table: "AioRequestPeopleInOut",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "NoteInId",
                table: "AioRequestAssetInOut",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoteInId",
                table: "AioRequestPeopleInOut");

            migrationBuilder.DropColumn(
                name: "NoteInId",
                table: "AioRequestAssetInOut");

            migrationBuilder.RenameColumn(
                name: "NoteOutId",
                table: "AioRequestPeopleInOut",
                newName: "NoteId");

            migrationBuilder.RenameColumn(
                name: "NoteOutId",
                table: "AioRequestAssetInOut",
                newName: "NoteId");
        }
    }
}

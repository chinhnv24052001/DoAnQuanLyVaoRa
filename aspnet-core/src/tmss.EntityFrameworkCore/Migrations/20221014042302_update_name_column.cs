using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class update_name_column : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PeopleId",
                table: "AioRequestPeopleInOut",
                newName: "RequestPeopleId");

            migrationBuilder.RenameColumn(
                name: "RequestAssetId",
                table: "AioRequestPeople",
                newName: "RequestId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "AioRequestAssetInOut",
                newName: "RequestAssetId");

            migrationBuilder.RenameColumn(
                name: "RequestAssetId",
                table: "AioRequestAsset",
                newName: "RequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RequestPeopleId",
                table: "AioRequestPeopleInOut",
                newName: "PeopleId");

            migrationBuilder.RenameColumn(
                name: "RequestId",
                table: "AioRequestPeople",
                newName: "RequestAssetId");

            migrationBuilder.RenameColumn(
                name: "RequestAssetId",
                table: "AioRequestAssetInOut",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "RequestId",
                table: "AioRequestAsset",
                newName: "RequestAssetId");
        }
    }
}

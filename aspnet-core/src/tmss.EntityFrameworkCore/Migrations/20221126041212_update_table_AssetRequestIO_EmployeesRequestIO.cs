using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class update_table_AssetRequestIO_EmployeesRequestIO : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOut",
                table: "AioRequestPeople");

            migrationBuilder.DropColumn(
                name: "IsOut",
                table: "AioRequestAsset");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOut",
                table: "AioRequestPeople",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOut",
                table: "AioRequestAsset",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}

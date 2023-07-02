using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class update_table_courcesafety : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EffectiveDate",
                table: "MstCourceSafety",
                newName: "EffectiveDateStart");

            migrationBuilder.AddColumn<DateTime>(
                name: "EffectiveDateEnd",
                table: "MstCourceSafety",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EffectiveDateEnd",
                table: "MstCourceSafety");

            migrationBuilder.RenameColumn(
                name: "EffectiveDateStart",
                table: "MstCourceSafety",
                newName: "EffectiveDate");
        }
    }
}

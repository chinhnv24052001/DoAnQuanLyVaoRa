﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class Update_table_MstAsset_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TagCode",
                table: "MstAsset");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TagCode",
                table: "MstAsset",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

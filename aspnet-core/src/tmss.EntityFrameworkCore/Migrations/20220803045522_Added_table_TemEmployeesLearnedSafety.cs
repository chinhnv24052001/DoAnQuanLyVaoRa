using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class Added_table_TemEmployeesLearnedSafety : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MstEmployeesLearnedSafetys",
                table: "MstEmployeesLearnedSafetys");

            migrationBuilder.RenameTable(
                name: "MstEmployeesLearnedSafetys",
                newName: "MstEmployeesLearnedSafety");

            migrationBuilder.AlterColumn<string>(
                name: "CourceCode",
                table: "MstCourceSafety",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "CourceCode",
                table: "MstEmployeesLearnedSafety",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MstEmployeesLearnedSafety",
                table: "MstEmployeesLearnedSafety",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "TemEmployeesLearnedSafety",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Seq = table.Column<long>(type: "bigint", nullable: true),
                    EmployeesName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IdentityCard = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    CourceCode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Validate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemEmployeesLearnedSafety", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TemEmployeesLearnedSafety");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MstEmployeesLearnedSafety",
                table: "MstEmployeesLearnedSafety");

            migrationBuilder.RenameTable(
                name: "MstEmployeesLearnedSafety",
                newName: "MstEmployeesLearnedSafetys");

            migrationBuilder.AlterColumn<string>(
                name: "CourceCode",
                table: "MstCourceSafety",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "CourceCode",
                table: "MstEmployeesLearnedSafetys",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MstEmployeesLearnedSafetys",
                table: "MstEmployeesLearnedSafetys",
                column: "Id");
        }
    }
}

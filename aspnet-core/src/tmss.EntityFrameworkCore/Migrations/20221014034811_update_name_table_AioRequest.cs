using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class update_name_table_AioRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AioRequestAssetBring");

            migrationBuilder.CreateTable(
                name: "AioRequest",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DateRequest = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DateManageApproval = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateTemManageApproval = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ManageApproval = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VenderId = table.Column<long>(type: "bigint", nullable: true),
                    DateAdminApproval = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AdminApprovalId = table.Column<long>(type: "bigint", nullable: true),
                    TypeRequest = table.Column<long>(type: "bigint", nullable: true),
                    RequestCode = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<long>(type: "bigint", nullable: true),
                    StatusDraft = table.Column<int>(type: "int", nullable: false),
                    TemManagerApproval = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TemManageIntervent = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_AioRequest", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AioRequest");

            migrationBuilder.CreateTable(
                name: "AioRequestAssetBring",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Adm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdminApprovalId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DateAdminApproval = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateManageApproval = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateRequest = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateTemManageApproval = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    ManageApproval = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestCode = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<long>(type: "bigint", maxLength: 500, nullable: true),
                    StatusDraft = table.Column<int>(type: "int", nullable: false),
                    TemManageIntervent = table.Column<bool>(type: "bit", nullable: false),
                    TemManagerApproval = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TypeRequest = table.Column<long>(type: "bigint", nullable: true),
                    VenderId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AioRequestAssetBring", x => x.Id);
                });
        }
    }
}

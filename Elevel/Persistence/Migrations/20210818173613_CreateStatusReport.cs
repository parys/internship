using Microsoft.EntityFrameworkCore.Migrations;

namespace Elevel.Infrastructure.Persistence.Migrations
{
    public partial class CreateStatusReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSolved",
                table: "Reports");

            migrationBuilder.AddColumn<byte>(
                name: "ReportStatus",
                table: "Reports",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReportStatus",
                table: "Reports");

            migrationBuilder.AddColumn<bool>(
                name: "IsSolved",
                table: "Reports",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}

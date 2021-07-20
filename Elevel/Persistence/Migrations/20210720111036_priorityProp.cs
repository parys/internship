using Microsoft.EntityFrameworkCore.Migrations;

namespace Elevel.Infrastructure.Persistence.Migrations
{
    public partial class priorityProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "Test",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Test");
        }
    }
}

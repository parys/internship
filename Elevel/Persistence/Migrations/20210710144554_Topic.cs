using Microsoft.EntityFrameworkCore.Migrations;

namespace Elevel.Infrastructure.Persistence.Migrations
{
    public partial class Topic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tests_Topic_EssayId",
                table: "Tests");

            migrationBuilder.DropForeignKey(
                name: "FK_Tests_Topic_SpeakingId",
                table: "Tests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Topic",
                table: "Topic");

            migrationBuilder.RenameTable(
                name: "Topic",
                newName: "Topics");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Topics",
                table: "Topics",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_Topics_EssayId",
                table: "Tests",
                column: "EssayId",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_Topics_SpeakingId",
                table: "Tests",
                column: "SpeakingId",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tests_Topics_EssayId",
                table: "Tests");

            migrationBuilder.DropForeignKey(
                name: "FK_Tests_Topics_SpeakingId",
                table: "Tests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Topics",
                table: "Topics");

            migrationBuilder.RenameTable(
                name: "Topics",
                newName: "Topic");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Topic",
                table: "Topic",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_Topic_EssayId",
                table: "Tests",
                column: "EssayId",
                principalTable: "Topic",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_Topic_SpeakingId",
                table: "Tests",
                column: "SpeakingId",
                principalTable: "Topic",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

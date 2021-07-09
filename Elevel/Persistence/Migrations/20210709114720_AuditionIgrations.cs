using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elevel.Infrastructure.Persistence.Migrations
{
    public partial class AuditionIgrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserAnswerId",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "AnswerId",
                table: "Answers");

            migrationBuilder.CreateTable(
                name: "Auditions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuditionAudio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auditions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Topic",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    TopicName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topic", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tests_AuditionId",
                table: "Tests",
                column: "AuditionId");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_CoachId",
                table: "Tests",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_EssayId",
                table: "Tests",
                column: "EssayId");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_HrId",
                table: "Tests",
                column: "HrId");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_SpeakingId",
                table: "Tests",
                column: "SpeakingId");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_UserId",
                table: "Tests",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_AspNetUsers_CoachId",
                table: "Tests",
                column: "CoachId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_AspNetUsers_HrId",
                table: "Tests",
                column: "HrId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_AspNetUsers_UserId",
                table: "Tests",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_Auditions_AuditionId",
                table: "Tests",
                column: "AuditionId",
                principalTable: "Auditions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tests_AspNetUsers_CoachId",
                table: "Tests");

            migrationBuilder.DropForeignKey(
                name: "FK_Tests_AspNetUsers_HrId",
                table: "Tests");

            migrationBuilder.DropForeignKey(
                name: "FK_Tests_AspNetUsers_UserId",
                table: "Tests");

            migrationBuilder.DropForeignKey(
                name: "FK_Tests_Auditions_AuditionId",
                table: "Tests");

            migrationBuilder.DropForeignKey(
                name: "FK_Tests_Topic_EssayId",
                table: "Tests");

            migrationBuilder.DropForeignKey(
                name: "FK_Tests_Topic_SpeakingId",
                table: "Tests");

            migrationBuilder.DropTable(
                name: "Auditions");

            migrationBuilder.DropTable(
                name: "Topic");

            migrationBuilder.DropIndex(
                name: "IX_Tests_AuditionId",
                table: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_Tests_CoachId",
                table: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_Tests_EssayId",
                table: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_Tests_HrId",
                table: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_Tests_SpeakingId",
                table: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_Tests_UserId",
                table: "Tests");

            migrationBuilder.AddColumn<Guid>(
                name: "UserAnswerId",
                table: "UserAnswers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "QuestionId",
                table: "Questions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "AnswerId",
                table: "Answers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}

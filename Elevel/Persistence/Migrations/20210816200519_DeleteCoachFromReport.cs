using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elevel.Infrastructure.Persistence.Migrations
{
    public partial class DeleteCoachFromReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_AspNetUsers_CoachId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_CoachId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "CoachId",
                table: "Reports");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "Reports",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_UserId1",
                table: "Reports",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_AspNetUsers_UserId1",
                table: "Reports",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_AspNetUsers_UserId1",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_UserId1",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Reports");

            migrationBuilder.AddColumn<Guid>(
                name: "CoachId",
                table: "Reports",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Reports_CoachId",
                table: "Reports",
                column: "CoachId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_AspNetUsers_CoachId",
                table: "Reports",
                column: "CoachId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

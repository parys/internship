using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elevel.Infrastructure.Persistence.Migrations
{
    public partial class AddCreatorIdForReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "Reports",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Topics_CreatorId",
                table: "Topics",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_CreatorId",
                table: "Reports",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_CreatorId",
                table: "Questions",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Auditions_CreatorId",
                table: "Auditions",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Auditions_AspNetUsers_CreatorId",
                table: "Auditions",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_AspNetUsers_CreatorId",
                table: "Questions",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_AspNetUsers_CreatorId",
                table: "Reports",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Topics_AspNetUsers_CreatorId",
                table: "Topics",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auditions_AspNetUsers_CreatorId",
                table: "Auditions");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_AspNetUsers_CreatorId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_AspNetUsers_CreatorId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Topics_AspNetUsers_CreatorId",
                table: "Topics");

            migrationBuilder.DropIndex(
                name: "IX_Topics_CreatorId",
                table: "Topics");

            migrationBuilder.DropIndex(
                name: "IX_Reports_CreatorId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Questions_CreatorId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Auditions_CreatorId",
                table: "Auditions");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Reports");
        }
    }
}

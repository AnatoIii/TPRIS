using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenLibraryServer.DataAccess.Migrations
{
    public partial class chats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ChatId",
                table: "Books",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_ChatId",
                table: "Books",
                column: "ChatId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Chats_ChatId",
                table: "Books",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "ChatId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Chats_ChatId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_ChatId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "Books");
        }
    }
}

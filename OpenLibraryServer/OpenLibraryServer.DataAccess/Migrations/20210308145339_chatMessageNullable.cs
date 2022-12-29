using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenLibraryServer.DataAccess.Migrations
{
    public partial class chatMessageNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ChatMessageId1",
                table: "Chats",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Chats_ChatMessageId1",
                table: "Chats",
                column: "ChatMessageId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_ChatMessages_ChatMessageId1",
                table: "Chats",
                column: "ChatMessageId1",
                principalTable: "ChatMessages",
                principalColumn: "ChatMessageId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_ChatMessages_ChatMessageId1",
                table: "Chats");

            migrationBuilder.DropIndex(
                name: "IX_Chats_ChatMessageId1",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "ChatMessageId1",
                table: "Chats");
        }
    }
}

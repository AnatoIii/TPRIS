using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenLibraryServer.DataAccess.Migrations
{
    public partial class chatMessageFinal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_Chats_ChatId",
                table: "ChatMessages");

            migrationBuilder.DropIndex(
                name: "IX_ChatMessages_ChatId",
                table: "ChatMessages");

            migrationBuilder.AddColumn<Guid>(
                name: "ChatMessageId",
                table: "Chats",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Chats_ChatMessageId",
                table: "Chats",
                column: "ChatMessageId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_ChatMessages_ChatMessageId",
                table: "Chats",
                column: "ChatMessageId",
                principalTable: "ChatMessages",
                principalColumn: "ChatMessageId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_ChatMessages_ChatMessageId",
                table: "Chats");

            migrationBuilder.DropIndex(
                name: "IX_Chats_ChatMessageId",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "ChatMessageId",
                table: "Chats");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_ChatId",
                table: "ChatMessages",
                column: "ChatId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_Chats_ChatId",
                table: "ChatMessages",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "ChatId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

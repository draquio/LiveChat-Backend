using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LiveChat.Migrations
{
    /// <inheritdoc />
    public partial class changeChatRoomWithParticipants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_ChatRooms_ChatRoomId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ChatRoomId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ChatRoomId",
                table: "Messages");

            migrationBuilder.CreateTable(
                name: "ChatParticipant",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GuestName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChatRoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatParticipant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatParticipant_ChatRooms_ChatRoomId",
                        column: x => x.ChatRoomId,
                        principalTable: "ChatRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatParticipant_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_RoomId",
                table: "Messages",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatParticipant_ChatRoomId",
                table: "ChatParticipant",
                column: "ChatRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatParticipant_UserId",
                table: "ChatParticipant",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_ChatRooms_RoomId",
                table: "Messages",
                column: "RoomId",
                principalTable: "ChatRooms",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_ChatRooms_RoomId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "ChatParticipant");

            migrationBuilder.DropIndex(
                name: "IX_Messages_RoomId",
                table: "Messages");

            migrationBuilder.AddColumn<Guid>(
                name: "ChatRoomId",
                table: "Messages",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ChatRoomId",
                table: "Messages",
                column: "ChatRoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_ChatRooms_ChatRoomId",
                table: "Messages",
                column: "ChatRoomId",
                principalTable: "ChatRooms",
                principalColumn: "Id");
        }
    }
}

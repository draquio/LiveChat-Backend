using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LiveChat.Migrations
{
    /// <inheritdoc />
    public partial class RemoveChatName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "name",
                table: "ChatRooms");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "ChatRooms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

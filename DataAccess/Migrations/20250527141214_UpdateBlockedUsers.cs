using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccesses.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBlockedUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlockedUser_User_UserId",
                table: "BlockedUser");

            migrationBuilder.DropIndex(
                name: "IX_BlockedUser_UserId",
                table: "BlockedUser");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BlockedUser");

            migrationBuilder.CreateIndex(
                name: "IX_BlockedUser_UserId1",
                table: "BlockedUser",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_BlockedUser_User_UserId1",
                table: "BlockedUser",
                column: "UserId1",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlockedUser_User_UserId1",
                table: "BlockedUser");

            migrationBuilder.DropIndex(
                name: "IX_BlockedUser_UserId1",
                table: "BlockedUser");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "BlockedUser",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BlockedUser_UserId",
                table: "BlockedUser",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlockedUser_User_UserId",
                table: "BlockedUser",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

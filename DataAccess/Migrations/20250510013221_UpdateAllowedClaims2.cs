using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccesses.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAllowedClaims2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AllowedUser",
                table: "AllowedUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AllowedRole",
                table: "AllowedRole");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AllowedUser",
                table: "AllowedUser",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AllowedRole",
                table: "AllowedRole",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AllowedUser_ChannelId",
                table: "AllowedUser",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_AllowedRole_ChannelId",
                table: "AllowedRole",
                column: "ChannelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AllowedUser",
                table: "AllowedUser");

            migrationBuilder.DropIndex(
                name: "IX_AllowedUser_ChannelId",
                table: "AllowedUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AllowedRole",
                table: "AllowedRole");

            migrationBuilder.DropIndex(
                name: "IX_AllowedRole_ChannelId",
                table: "AllowedRole");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AllowedUser",
                table: "AllowedUser",
                columns: new[] { "ChannelId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AllowedRole",
                table: "AllowedRole",
                columns: new[] { "ChannelId", "RoleId" });
        }
    }
}

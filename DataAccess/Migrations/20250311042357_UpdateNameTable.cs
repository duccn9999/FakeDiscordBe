using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccesses.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNameTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupChatRoles_GroupChat_GroupChatId",
                table: "GroupChatRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupChatRoles_Role_RoleId",
                table: "GroupChatRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupChatRoles",
                table: "GroupChatRoles");

            migrationBuilder.RenameTable(
                name: "GroupChatRoles",
                newName: "GroupChatRole");

            migrationBuilder.RenameIndex(
                name: "IX_GroupChatRoles_RoleId",
                table: "GroupChatRole",
                newName: "IX_GroupChatRole_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupChatRole",
                table: "GroupChatRole",
                columns: new[] { "GroupChatId", "RoleId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GroupChatRole_GroupChat_GroupChatId",
                table: "GroupChatRole",
                column: "GroupChatId",
                principalTable: "GroupChat",
                principalColumn: "GroupChatId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupChatRole_Role_RoleId",
                table: "GroupChatRole",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupChatRole_GroupChat_GroupChatId",
                table: "GroupChatRole");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupChatRole_Role_RoleId",
                table: "GroupChatRole");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupChatRole",
                table: "GroupChatRole");

            migrationBuilder.RenameTable(
                name: "GroupChatRole",
                newName: "GroupChatRoles");

            migrationBuilder.RenameIndex(
                name: "IX_GroupChatRole_RoleId",
                table: "GroupChatRoles",
                newName: "IX_GroupChatRoles_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupChatRoles",
                table: "GroupChatRoles",
                columns: new[] { "GroupChatId", "RoleId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GroupChatRoles_GroupChat_GroupChatId",
                table: "GroupChatRoles",
                column: "GroupChatId",
                principalTable: "GroupChat",
                principalColumn: "GroupChatId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupChatRoles_Role_RoleId",
                table: "GroupChatRoles",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

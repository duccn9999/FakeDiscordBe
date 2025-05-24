using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccesses.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSuspendUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SuspenUser_SuperAdmin_SuperAdminId",
                table: "SuspenUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SuspenUser",
                table: "SuspenUser");

            migrationBuilder.RenameTable(
                name: "SuspenUser",
                newName: "SuspendUser");

            migrationBuilder.RenameIndex(
                name: "IX_SuspenUser_SuperAdminId",
                table: "SuspendUser",
                newName: "IX_SuspendUser_SuperAdminId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SuspendUser",
                table: "SuspendUser",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SuspendUser_SuperAdmin_SuperAdminId",
                table: "SuspendUser",
                column: "SuperAdminId",
                principalTable: "SuperAdmin",
                principalColumn: "SuperAdminId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SuspendUser_SuperAdmin_SuperAdminId",
                table: "SuspendUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SuspendUser",
                table: "SuspendUser");

            migrationBuilder.RenameTable(
                name: "SuspendUser",
                newName: "SuspenUser");

            migrationBuilder.RenameIndex(
                name: "IX_SuspendUser_SuperAdminId",
                table: "SuspenUser",
                newName: "IX_SuspenUser_SuperAdminId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SuspenUser",
                table: "SuspenUser",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SuspenUser_SuperAdmin_SuperAdminId",
                table: "SuspenUser",
                column: "SuperAdminId",
                principalTable: "SuperAdmin",
                principalColumn: "SuperAdminId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccesses.Migrations
{
    /// <inheritdoc />
    public partial class AddSuspendUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SuspenUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SuperAdminId = table.Column<int>(type: "int", nullable: false),
                    SuspendReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateSuspend = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuspenUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuspenUser_SuperAdmin_SuperAdminId",
                        column: x => x.SuperAdminId,
                        principalTable: "SuperAdmin",
                        principalColumn: "SuperAdminId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SuspenUser_SuperAdminId",
                table: "SuspenUser",
                column: "SuperAdminId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SuspenUser");
        }
    }
}

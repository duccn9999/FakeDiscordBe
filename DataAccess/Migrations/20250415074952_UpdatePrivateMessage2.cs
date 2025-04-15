using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccesses.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePrivateMessage2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReplyTo",
                table: "PrivateMessage");

            migrationBuilder.AddColumn<int>(
                name: "Receiver",
                table: "PrivateMessage",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Receiver",
                table: "PrivateMessage");

            migrationBuilder.AddColumn<int>(
                name: "ReplyTo",
                table: "PrivateMessage",
                type: "int",
                nullable: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccesses.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePrivateMessageAttachment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "PrivateMessageAttachment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DownloadLink",
                table: "PrivateMessageAttachment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OriginalFilename",
                table: "PrivateMessageAttachment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PublicId",
                table: "PrivateMessageAttachment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "PrivateMessageAttachment");

            migrationBuilder.DropColumn(
                name: "DownloadLink",
                table: "PrivateMessageAttachment");

            migrationBuilder.DropColumn(
                name: "OriginalFilename",
                table: "PrivateMessageAttachment");

            migrationBuilder.DropColumn(
                name: "PublicId",
                table: "PrivateMessageAttachment");
        }
    }
}

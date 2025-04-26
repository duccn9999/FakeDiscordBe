using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccesses.Migrations
{
    /// <inheritdoc />
    public partial class AddDownloadLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DownloadLink",
                table: "PrivateMessageAttachment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DownloadLink",
                table: "PrivateMessageAttachment");
        }
    }
}

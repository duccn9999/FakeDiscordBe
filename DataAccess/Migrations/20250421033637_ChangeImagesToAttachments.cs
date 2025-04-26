using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccesses.Migrations
{
    /// <inheritdoc />
    public partial class ChangeImagesToAttachments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrivateMessageImage");

            migrationBuilder.CreateTable(
                name: "PrivateMessageAttachment",
                columns: table => new
                {
                    AttachmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MessageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivateMessageAttachment", x => x.AttachmentId);
                    table.ForeignKey(
                        name: "FK_PrivateMessageAttachment_PrivateMessage_MessageId",
                        column: x => x.MessageId,
                        principalTable: "PrivateMessage",
                        principalColumn: "MessageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrivateMessageAttachment_MessageId",
                table: "PrivateMessageAttachment",
                column: "MessageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrivateMessageAttachment");

            migrationBuilder.CreateTable(
                name: "PrivateMessageImage",
                columns: table => new
                {
                    ImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivateMessageImage", x => x.ImageId);
                    table.ForeignKey(
                        name: "FK_PrivateMessageImage_PrivateMessage_MessageId",
                        column: x => x.MessageId,
                        principalTable: "PrivateMessage",
                        principalColumn: "MessageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrivateMessageImage_MessageId",
                table: "PrivateMessageImage",
                column: "MessageId");
        }
    }
}

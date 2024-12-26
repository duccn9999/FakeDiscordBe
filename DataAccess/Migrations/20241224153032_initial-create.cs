using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccesses.Migrations
{
    /// <inheritdoc />
    public partial class initialcreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GroupChat",
                columns: table => new
                {
                    GroupChatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoverImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserCreated = table.Column<int>(type: "int", nullable: false),
                    UserModified = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupChat", x => x.GroupChatId);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserCreated = table.Column<int>(type: "int", nullable: false),
                    UserModified = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoverImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "GroupChatParticipation",
                columns: table => new
                {
                    GroupChatParticipationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    GroupChatId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    NickName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateJoined = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupChatParticipation", x => x.GroupChatParticipationId);
                    table.ForeignKey(
                        name: "FK_GroupChatParticipation_GroupChat_GroupChatId",
                        column: x => x.GroupChatId,
                        principalTable: "GroupChat",
                        principalColumn: "GroupChatId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupChatParticipation_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupChatParticipation_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrivateMessage",
                columns: table => new
                {
                    PrivateMsgId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ReplyTo = table.Column<int>(type: "int", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivateMessage", x => x.PrivateMsgId);
                    table.ForeignKey(
                        name: "FK_PrivateMessage_PrivateMessage_ReplyTo",
                        column: x => x.ReplyTo,
                        principalTable: "PrivateMessage",
                        principalColumn: "PrivateMsgId");
                    table.ForeignKey(
                        name: "FK_PrivateMessage_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupMessage",
                columns: table => new
                {
                    GroupMsgId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserCreated = table.Column<int>(type: "int", nullable: false),
                    ReplyTo = table.Column<int>(type: "int", nullable: true),
                    GroupChatParticipationId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMessage", x => x.GroupMsgId);
                    table.ForeignKey(
                        name: "FK_GroupMessage_GroupChatParticipation_GroupChatParticipationId",
                        column: x => x.GroupChatParticipationId,
                        principalTable: "GroupChatParticipation",
                        principalColumn: "GroupChatParticipationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupMessage_GroupMessage_ReplyTo",
                        column: x => x.ReplyTo,
                        principalTable: "GroupMessage",
                        principalColumn: "GroupMsgId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupChatParticipation_GroupChatId",
                table: "GroupChatParticipation",
                column: "GroupChatId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupChatParticipation_RoleId",
                table: "GroupChatParticipation",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupChatParticipation_UserId",
                table: "GroupChatParticipation",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMessage_GroupChatParticipationId",
                table: "GroupMessage",
                column: "GroupChatParticipationId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMessage_ReplyTo",
                table: "GroupMessage",
                column: "ReplyTo");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateMessage_ReplyTo",
                table: "PrivateMessage",
                column: "ReplyTo");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateMessage_UserId",
                table: "PrivateMessage",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupMessage");

            migrationBuilder.DropTable(
                name: "PrivateMessage");

            migrationBuilder.DropTable(
                name: "GroupChatParticipation");

            migrationBuilder.DropTable(
                name: "GroupChat");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}

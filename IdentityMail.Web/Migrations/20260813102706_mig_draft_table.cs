using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityMail.Web.Migrations
{
    /// <inheritdoc />
    public partial class mig_draft_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDraft",
                table: "UserMessages");

            migrationBuilder.AlterColumn<bool>(
                name: "IsImportant",
                table: "UserMessages",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "UserMessages",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Drafts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    ReceiverEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    ParentMessageId = table.Column<int>(type: "int", nullable: true),
                    LastEditedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drafts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Drafts_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Drafts_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Drafts_UserMessages_ParentMessageId",
                        column: x => x.ParentMessageId,
                        principalTable: "UserMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Drafts_CategoryId",
                table: "Drafts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Drafts_ParentMessageId",
                table: "Drafts",
                column: "ParentMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_Drafts_SenderId",
                table: "Drafts",
                column: "SenderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Drafts");

            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "UserMessages");

            migrationBuilder.AlterColumn<bool>(
                name: "IsImportant",
                table: "UserMessages",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDraft",
                table: "UserMessages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}

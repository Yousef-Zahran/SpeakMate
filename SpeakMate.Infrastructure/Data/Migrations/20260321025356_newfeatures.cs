using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpeakMate.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class newfeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "LanguageIsCorrect",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ForeignLanguageLevel",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailableToChat",
                table: "Members",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LearningGoal",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MemberStats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TotalMessagesSent = table.Column<int>(type: "int", nullable: false),
                    CorrectionsReceived = table.Column<int>(type: "int", nullable: false),
                    CorrectionsAccepted = table.Column<int>(type: "int", nullable: false),
                    CurrentStreak = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberStats_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MessageCorrections",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MessageId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CorrectedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CorrectedText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Explanation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAccepted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageCorrections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageCorrections_Members_CorrectedById",
                        column: x => x.CorrectedById,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MessageCorrections_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SavedWords",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MemberId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Word = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Context = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Translation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SavedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedWords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SavedWords_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SavedWords_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MemberStats_MemberId",
                table: "MemberStats",
                column: "MemberId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MessageCorrections_CorrectedById",
                table: "MessageCorrections",
                column: "CorrectedById");

            migrationBuilder.CreateIndex(
                name: "IX_MessageCorrections_MessageId",
                table: "MessageCorrections",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedWords_MemberId",
                table: "SavedWords",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedWords_MessageId",
                table: "SavedWords",
                column: "MessageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberStats");

            migrationBuilder.DropTable(
                name: "MessageCorrections");

            migrationBuilder.DropTable(
                name: "SavedWords");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "LanguageIsCorrect",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ForeignLanguageLevel",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "IsAvailableToChat",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "LearningGoal",
                table: "Members");
        }
    }
}

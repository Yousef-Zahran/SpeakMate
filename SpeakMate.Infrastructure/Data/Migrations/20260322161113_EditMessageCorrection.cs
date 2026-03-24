using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpeakMate.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditMessageCorrection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CorrectPartEnd",
                table: "MessageCorrections",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CorrectPartStart",
                table: "MessageCorrections",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WrongPartEnd",
                table: "MessageCorrections",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WrongPartStart",
                table: "MessageCorrections",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrectPartEnd",
                table: "MessageCorrections");

            migrationBuilder.DropColumn(
                name: "CorrectPartStart",
                table: "MessageCorrections");

            migrationBuilder.DropColumn(
                name: "WrongPartEnd",
                table: "MessageCorrections");

            migrationBuilder.DropColumn(
                name: "WrongPartStart",
                table: "MessageCorrections");
        }
    }
}

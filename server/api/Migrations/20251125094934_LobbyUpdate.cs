using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class LobbyUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Lobbies",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "MentorId",
                table: "Lobbies",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lobbies_MentorId",
                table: "Lobbies",
                column: "MentorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lobbies_Mentors_MentorId",
                table: "Lobbies",
                column: "MentorId",
                principalTable: "Mentors",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lobbies_Mentors_MentorId",
                table: "Lobbies");

            migrationBuilder.DropIndex(
                name: "IX_Lobbies_MentorId",
                table: "Lobbies");

            migrationBuilder.DropColumn(
                name: "MentorId",
                table: "Lobbies");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Lobbies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}

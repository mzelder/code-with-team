using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class TasksFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LobbyMembers_UserTaskProgressId",
                table: "LobbyMembers");

            migrationBuilder.DropIndex(
                name: "IX_Lobbies_TeamTaskProgressId",
                table: "Lobbies");

            migrationBuilder.CreateIndex(
                name: "IX_LobbyMembers_UserTaskProgressId",
                table: "LobbyMembers",
                column: "UserTaskProgressId",
                unique: true,
                filter: "[UserTaskProgressId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Lobbies_TeamTaskProgressId",
                table: "Lobbies",
                column: "TeamTaskProgressId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LobbyMembers_UserTaskProgressId",
                table: "LobbyMembers");

            migrationBuilder.DropIndex(
                name: "IX_Lobbies_TeamTaskProgressId",
                table: "Lobbies");

            migrationBuilder.CreateIndex(
                name: "IX_LobbyMembers_UserTaskProgressId",
                table: "LobbyMembers",
                column: "UserTaskProgressId");

            migrationBuilder.CreateIndex(
                name: "IX_Lobbies_TeamTaskProgressId",
                table: "Lobbies",
                column: "TeamTaskProgressId");
        }
    }
}

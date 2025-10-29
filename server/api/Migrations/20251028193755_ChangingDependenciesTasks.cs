using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class ChangingDependenciesTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lobbies_TeamTaskProgresses_TeamTaskProgressId",
                table: "Lobbies");

            migrationBuilder.DropForeignKey(
                name: "FK_LobbyMembers_UserTaskProgresses_UserTaskProgressId",
                table: "LobbyMembers");

            migrationBuilder.DropIndex(
                name: "IX_LobbyMembers_UserTaskProgressId",
                table: "LobbyMembers");

            migrationBuilder.DropIndex(
                name: "IX_Lobbies_TeamTaskProgressId",
                table: "Lobbies");

            migrationBuilder.DropColumn(
                name: "CreatedIssues",
                table: "UserTaskProgresses");

            migrationBuilder.DropColumn(
                name: "UserTaskProgressId",
                table: "LobbyMembers");

            migrationBuilder.AddColumn<int>(
                name: "LobbyMemberId",
                table: "UserTaskProgresses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LobbyId",
                table: "TeamTaskProgresses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserTaskProgresses_LobbyMemberId",
                table: "UserTaskProgresses",
                column: "LobbyMemberId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeamTaskProgresses_LobbyId",
                table: "TeamTaskProgresses",
                column: "LobbyId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamTaskProgresses_Lobbies_LobbyId",
                table: "TeamTaskProgresses",
                column: "LobbyId",
                principalTable: "Lobbies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTaskProgresses_LobbyMembers_LobbyMemberId",
                table: "UserTaskProgresses",
                column: "LobbyMemberId",
                principalTable: "LobbyMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamTaskProgresses_Lobbies_LobbyId",
                table: "TeamTaskProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTaskProgresses_LobbyMembers_LobbyMemberId",
                table: "UserTaskProgresses");

            migrationBuilder.DropIndex(
                name: "IX_UserTaskProgresses_LobbyMemberId",
                table: "UserTaskProgresses");

            migrationBuilder.DropIndex(
                name: "IX_TeamTaskProgresses_LobbyId",
                table: "TeamTaskProgresses");

            migrationBuilder.DropColumn(
                name: "LobbyMemberId",
                table: "UserTaskProgresses");

            migrationBuilder.DropColumn(
                name: "LobbyId",
                table: "TeamTaskProgresses");

            migrationBuilder.AddColumn<bool>(
                name: "CreatedIssues",
                table: "UserTaskProgresses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "UserTaskProgressId",
                table: "LobbyMembers",
                type: "int",
                nullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Lobbies_TeamTaskProgresses_TeamTaskProgressId",
                table: "Lobbies",
                column: "TeamTaskProgressId",
                principalTable: "TeamTaskProgresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LobbyMembers_UserTaskProgresses_UserTaskProgressId",
                table: "LobbyMembers",
                column: "UserTaskProgressId",
                principalTable: "UserTaskProgresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

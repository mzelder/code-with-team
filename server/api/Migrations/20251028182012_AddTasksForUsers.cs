using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class AddTasksForUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserTaskProgressId",
                table: "LobbyMembers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeamTaskProgressId",
                table: "Lobbies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TeamTaskProgresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedIssues = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamTaskProgresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserTaskProgresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JoinedVideoCall = table.Column<bool>(type: "bit", nullable: false),
                    VisitedRepo = table.Column<bool>(type: "bit", nullable: false),
                    CreatedIssues = table.Column<bool>(type: "bit", nullable: false),
                    StartedCoding = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTaskProgresses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LobbyMembers_UserTaskProgressId",
                table: "LobbyMembers",
                column: "UserTaskProgressId");

            migrationBuilder.CreateIndex(
                name: "IX_Lobbies_TeamTaskProgressId",
                table: "Lobbies",
                column: "TeamTaskProgressId");

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
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lobbies_TeamTaskProgresses_TeamTaskProgressId",
                table: "Lobbies");

            migrationBuilder.DropForeignKey(
                name: "FK_LobbyMembers_UserTaskProgresses_UserTaskProgressId",
                table: "LobbyMembers");

            migrationBuilder.DropTable(
                name: "TeamTaskProgresses");

            migrationBuilder.DropTable(
                name: "UserTaskProgresses");

            migrationBuilder.DropIndex(
                name: "IX_LobbyMembers_UserTaskProgressId",
                table: "LobbyMembers");

            migrationBuilder.DropIndex(
                name: "IX_Lobbies_TeamTaskProgressId",
                table: "Lobbies");

            migrationBuilder.DropColumn(
                name: "UserTaskProgressId",
                table: "LobbyMembers");

            migrationBuilder.DropColumn(
                name: "TeamTaskProgressId",
                table: "Lobbies");
        }
    }
}

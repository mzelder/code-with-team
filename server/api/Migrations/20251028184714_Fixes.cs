using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class Fixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LobbyMembers_UserTaskProgresses_UserTaskProgressId",
                table: "LobbyMembers");

            migrationBuilder.AddForeignKey(
                name: "FK_LobbyMembers_UserTaskProgresses_UserTaskProgressId",
                table: "LobbyMembers",
                column: "UserTaskProgressId",
                principalTable: "UserTaskProgresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LobbyMembers_UserTaskProgresses_UserTaskProgressId",
                table: "LobbyMembers");

            migrationBuilder.AddForeignKey(
                name: "FK_LobbyMembers_UserTaskProgresses_UserTaskProgressId",
                table: "LobbyMembers",
                column: "UserTaskProgressId",
                principalTable: "UserTaskProgresses",
                principalColumn: "Id");
        }
    }
}

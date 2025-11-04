using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class TaskDefinitionsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JoinedVideoCall",
                table: "UserTaskProgresses");

            migrationBuilder.DropColumn(
                name: "StartedCoding",
                table: "UserTaskProgresses");

            migrationBuilder.DropColumn(
                name: "VisitedRepo",
                table: "UserTaskProgresses");

            migrationBuilder.DropColumn(
                name: "CreatedIssues",
                table: "TeamTaskProgresses");

            migrationBuilder.CreateTable(
                name: "TaskDefinitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskDefinitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TeamTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeamTaskProgressId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamTasks_TeamTaskProgresses_TeamTaskProgressId",
                        column: x => x.TeamTaskProgressId,
                        principalTable: "TeamTaskProgresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserTaskProgressId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTasks_UserTaskProgresses_UserTaskProgressId",
                        column: x => x.UserTaskProgressId,
                        principalTable: "UserTaskProgresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "TaskDefinitions",
                columns: new[] { "Id", "Category", "Description", "IsCompleted", "Name" },
                values: new object[,]
                {
                    { 1, 1, "", false, "Book meeting" },
                    { 2, 1, "", false, "Attend your scheduled team meeting" },
                    { 3, 1, "", false, "Break down the tasks" },
                    { 4, 0, "", false, "Visit github repository" },
                    { 5, 0, "", false, "Start coding" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeamTasks_TeamTaskProgressId",
                table: "TeamTasks",
                column: "TeamTaskProgressId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTasks_UserTaskProgressId",
                table: "UserTasks",
                column: "UserTaskProgressId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskDefinitions");

            migrationBuilder.DropTable(
                name: "TeamTasks");

            migrationBuilder.DropTable(
                name: "UserTasks");

            migrationBuilder.AddColumn<bool>(
                name: "JoinedVideoCall",
                table: "UserTaskProgresses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "StartedCoding",
                table: "UserTaskProgresses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "VisitedRepo",
                table: "UserTaskProgresses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CreatedIssues",
                table: "TeamTaskProgresses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}

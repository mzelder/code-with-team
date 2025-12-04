using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class RemovingFromTasksFinishedTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TaskDefinitions",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.AddColumn<bool>(
                name: "Finished",
                table: "LobbyMembers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Finished",
                table: "LobbyMembers");

            migrationBuilder.InsertData(
                table: "TaskDefinitions",
                columns: new[] { "Id", "Category", "Description", "IsCompleted", "Name" },
                values: new object[] { 5, 0, "", false, "Press finish button when you think project is finished" });
        }
    }
}

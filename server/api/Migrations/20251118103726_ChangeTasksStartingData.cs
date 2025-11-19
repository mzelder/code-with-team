using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTasksStartingData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TaskDefinitions",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Break down the tasks");

            migrationBuilder.UpdateData(
                table: "TaskDefinitions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Category", "Name" },
                values: new object[] { 0, "Attend your scheduled team meeting" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TaskDefinitions",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Attend your scheduled team meeting");

            migrationBuilder.UpdateData(
                table: "TaskDefinitions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Category", "Name" },
                values: new object[] { 1, "Break down the tasks" });
        }
    }
}

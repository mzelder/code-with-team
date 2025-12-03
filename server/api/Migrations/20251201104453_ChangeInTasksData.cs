using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class ChangeInTasksData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TaskDefinitions",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.UpdateData(
                table: "TaskDefinitions",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "Press finish button when you think project is finished");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TaskDefinitions",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "Start coding");

            migrationBuilder.InsertData(
                table: "TaskDefinitions",
                columns: new[] { "Id", "Category", "Description", "IsCompleted", "Name" },
                values: new object[] { 6, 0, "", false, "Press finish button when you think project is finished" });
        }
    }
}

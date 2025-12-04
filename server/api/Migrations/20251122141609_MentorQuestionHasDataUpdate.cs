using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class MentorQuestionHasDataUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MentorQuestions",
                columns: new[] { "Id", "QuestionText" },
                values: new object[,]
                {
                    { 1, "Describe a time you helped someone overcome a significant challenge in a project. What was your approach, and what did you learn from the experience?" },
                    { 2, "How do you balance giving guidance with allowing mentees to make their own decisions (and possible mistakes)? Please give a concrete example." },
                    { 3, "What types of mentees or situations do you find most challenging, and how do you adapt your mentoring style in those cases?" },
                    { 4, "How do you structure feedback (both positive and critical) during a project lifecycle? Include any frameworks or routines you use." },
                    { 5, "What motivates you to mentor, and what do you hope to gain or improve in yourself through mentoring this team?" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MentorQuestions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MentorQuestions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MentorQuestions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MentorQuestions",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "MentorQuestions",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}

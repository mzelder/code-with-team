using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMentorQuestionConnections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MentorAssesmentAnswers_MentorQuestions_MentorQuestionId",
                table: "MentorAssesmentAnswers");

            migrationBuilder.DropIndex(
                name: "IX_MentorAssesmentAnswers_MentorQuestionId",
                table: "MentorAssesmentAnswers");

            migrationBuilder.CreateIndex(
                name: "IX_MentorAssesmentAnswers_MentorQuestionId",
                table: "MentorAssesmentAnswers",
                column: "MentorQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_MentorAssesmentAnswers_MentorQuestions_MentorQuestionId",
                table: "MentorAssesmentAnswers",
                column: "MentorQuestionId",
                principalTable: "MentorQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MentorAssesmentAnswers_MentorQuestions_MentorQuestionId",
                table: "MentorAssesmentAnswers");

            migrationBuilder.DropIndex(
                name: "IX_MentorAssesmentAnswers_MentorQuestionId",
                table: "MentorAssesmentAnswers");

            migrationBuilder.CreateIndex(
                name: "IX_MentorAssesmentAnswers_MentorQuestionId",
                table: "MentorAssesmentAnswers",
                column: "MentorQuestionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MentorAssesmentAnswers_MentorQuestions_MentorQuestionId",
                table: "MentorAssesmentAnswers",
                column: "MentorQuestionId",
                principalTable: "MentorQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

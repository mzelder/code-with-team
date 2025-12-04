using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class AddMentorModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MentorForms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Motivation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MentorForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MentorForms_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MentorQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionText = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MentorQuestions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MentorPortfolioLinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MentorFormId = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MentorPortfolioLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MentorPortfolioLinks_MentorForms_MentorFormId",
                        column: x => x.MentorFormId,
                        principalTable: "MentorForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mentors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    MentorFormId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mentors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mentors_MentorForms_MentorFormId",
                        column: x => x.MentorFormId,
                        principalTable: "MentorForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Mentors_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MentorAssesmentAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnswerText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MentorQuestionId = table.Column<int>(type: "int", nullable: false),
                    MentorFormId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MentorAssesmentAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MentorAssesmentAnswers_MentorForms_MentorFormId",
                        column: x => x.MentorFormId,
                        principalTable: "MentorForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MentorAssesmentAnswers_MentorQuestions_MentorQuestionId",
                        column: x => x.MentorQuestionId,
                        principalTable: "MentorQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MentorAssesmentAnswers_MentorFormId",
                table: "MentorAssesmentAnswers",
                column: "MentorFormId");

            migrationBuilder.CreateIndex(
                name: "IX_MentorAssesmentAnswers_MentorQuestionId",
                table: "MentorAssesmentAnswers",
                column: "MentorQuestionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MentorForms_UserId",
                table: "MentorForms",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MentorPortfolioLinks_MentorFormId",
                table: "MentorPortfolioLinks",
                column: "MentorFormId");

            migrationBuilder.CreateIndex(
                name: "IX_Mentors_MentorFormId",
                table: "Mentors",
                column: "MentorFormId");

            migrationBuilder.CreateIndex(
                name: "IX_Mentors_UserId",
                table: "Mentors",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MentorAssesmentAnswers");

            migrationBuilder.DropTable(
                name: "MentorPortfolioLinks");

            migrationBuilder.DropTable(
                name: "Mentors");

            migrationBuilder.DropTable(
                name: "MentorQuestions");

            migrationBuilder.DropTable(
                name: "MentorForms");
        }
    }
}

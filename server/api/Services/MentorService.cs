using api.Data;
using api.Dtos.Mentor;
using api.Models.Mentor;
using api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace api.Services
{
    public class MentorService : IMentorService
    {
        private readonly AppDbContext _context;

        public MentorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<MentorDto> GetMentorStatusAsync(int userId)
        {
            var mentorDto = new MentorDto
            {
                IsMentor = await _context.Mentors.AnyAsync(m => m.UserId == userId),
                AppliedForMentor = await _context.MentorForms.AnyAsync(ma => ma.UserId == userId)
            };
            return mentorDto;
        }

        public async Task<MentorQuestionDto> GetMentorQuestionsAsync()
        {
            var questions = await _context.MentorQuestions
                .Select(q => q.QuestionText)
                .ToArrayAsync();
            return new MentorQuestionDto { Questions = questions };
        }

        public async Task SubmitMentorFormAsync(int userId, MentorFormDto mentorFormDto)
        {
            var existingForm = await _context.MentorForms
                .FirstOrDefaultAsync(mf => mf.UserId == userId);

            if (existingForm != null)
            {
                throw new InvalidOperationException("User has already submitted a mentor application.");
            }

            if (mentorFormDto == null)
            {
                throw new ArgumentNullException(nameof(mentorFormDto));
            }

            var questions = await _context.MentorQuestions.CountAsync();

            if (string.IsNullOrWhiteSpace(mentorFormDto.Fullname) ||
                string.IsNullOrWhiteSpace(mentorFormDto.Email) ||
                mentorFormDto.ScenarioAnswers.Length != questions)
            {
                throw new ArgumentException("All fields in the mentor form must be filled out.");
            }

            var mentorForm = new MentorForm
            {
                UserId = userId,
                FullName = mentorFormDto.Fullname,
                Email = mentorFormDto.Email,
                Motivation = mentorFormDto.Motivation,
                CreatedAt = DateTime.UtcNow,
                Status = MentorFormStatus.Pending,
            };

            mentorForm.MentorPortfolioLinks = mentorFormDto.PortfolioLinks
                .Select(link => new MentorPortfolioLink
                {
                    Url = link
                }).ToList();

            mentorForm.MentorAssesmentAnswers = mentorFormDto.ScenarioAnswers
                .Select((answer, index) => new MentorAssesmentAnswer
                {
                    MentorQuestionId = index + 1,
                    AnswerText = answer
                }).ToList();

            _context.MentorForms.Add(mentorForm);
            await _context.SaveChangesAsync();
        }

        public async Task AcceptMentorApplicationAsync(int mentorFormId)
        {
            var mentorForm = await _context.MentorForms
                .FirstOrDefaultAsync(mf => mf.Id == mentorFormId);

            if (mentorForm == null)
            {
                throw new InvalidOperationException("Mentor application not found.");
            }
            
            if (mentorForm.Status != MentorFormStatus.Pending)
            {
                throw new InvalidOperationException("Mentor application has already been processed.");
            }
            
            mentorForm.Status = MentorFormStatus.Approved;
            var mentor = new Mentor
            {
                UserId = mentorForm.UserId,
                CreatedAt = DateTime.UtcNow
            };
            _context.Mentors.Add(mentor);
            await _context.SaveChangesAsync();
        }
    }
}
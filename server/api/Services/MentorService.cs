using api.Data;
using api.Dtos.Matchmaking;
using api.Dtos.Mentor;
using api.Models;
using api.Models.Mentor;
using api.Models.Mentors;
using api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace api.Services
{
    public class MentorService : IMentorService
    {
        private readonly AppDbContext _context;
        private readonly IMatchmakingService _matchmakingService;

        public MentorService(AppDbContext context, IMatchmakingService matchmakingService)
        {
            _context = context;
            _matchmakingService = matchmakingService;
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

        public async Task<LobbyStatusDto[]> GetMentorTeams(int userId)
        {
            var mentor = await _context.Mentors
                .Where(m => m.UserId == userId)
                .FirstOrDefaultAsync()
            ?? throw new Exception("Mentor not found for the current user.");

            var lobbyRepresentiveIds = _context.LobbyMembers
                .Include(lm => lm.Lobby)
                .Where(lm => lm.Lobby.MentorId == mentor.Id)
                .GroupBy(lm => lm.Lobby)
                .Select(g => g.OrderBy(m => m.Id).Select(m => m.UserId).First())
                .ToListAsync();

            var mentorTeams = new List<LobbyStatusDto>();
            foreach (var id in lobbyRepresentiveIds.Result)
            {
                var lobbyStatus = await _matchmakingService.GetLobbyStatusAsync(id);
                mentorTeams.Add(lobbyStatus);
            }

            return mentorTeams.ToArray();
        }

        public async Task SubmitMentorFeedback(int userId, MentorReviewDto mentorReviewDto)
        {
            var mentor = await _context.Mentors
               .Where(m => m.UserId == userId)
               .FirstOrDefaultAsync()
           ?? throw new Exception("Mentor not found for the current user.");

            var lobby = await _context.Lobbies
                .Where(l => l.Id == mentorReviewDto.LobbyId)
                .Where(l => l.MentorId == mentor.Id)
                .FirstOrDefaultAsync()
            ?? throw new Exception("Lobby not found for the current mentor.");

            var mentorReview = new MentorReview
            {
                LobbyId = mentorReviewDto.LobbyId,
                MentorId = mentor.Id,
                Feedback = mentorReviewDto.Feedback,
                CreatedAt = DateTime.UtcNow
            };
            _context.MentorReviews.Add(mentorReview);
            await _context.SaveChangesAsync();
        }

        public async Task<MentorReviewDto> GetMentorFeedback(int userId)
        {
            var lobby = await _context.LobbyMembers
                .Include(lm => lm.Lobby)
                .Where(lm => lm.UserId == userId)
                .Select(lm => lm.Lobby)
                .FirstOrDefaultAsync()
            ?? throw new Exception("Lobby not found for the current user.");

            var mentorReview = await _context.MentorReviews
                .Where(mr => mr.LobbyId == lobby.Id)
                .FirstOrDefaultAsync()
            ?? throw new Exception("Mentor review not found for the current lobby.");

            return new MentorReviewDto
            {
                LobbyId = lobby.Id,
                Feedback = mentorReview.Feedback
            };
        }
    }
}
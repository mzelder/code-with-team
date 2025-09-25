using api.Data;
using api.Dtos;
using api.Dtos.Matchmaking;
using api.Models;
using api.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;


namespace api.Services
{
    public class MatchmakingService : IMatchmakingService
    {
        private readonly AppDbContext _context;

        public MatchmakingService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponseDto> StartQueueAsync(int userId, MatchmakingRequestDto dto)
        {
            var userInLobby = await _context.LobbyQueues
                .FirstOrDefaultAsync(l => l.UserId == userId);

            if (userInLobby != null)
            {
                _context.Remove(userInLobby);
            }

            var categoryExists = await _context.Categories.AnyAsync(c => c.Id == dto.CategoryId);
            var roleExists = await _context.Roles.AnyAsync(r => r.Id == dto.RoleId);
            var validLanguagesIds = await _context.ProgrammingLanguages.Select(p => p.Id).ToListAsync();
            bool allLanguagesValid = dto.ProgrammingLanguageIds.All(id => validLanguagesIds.Contains(id));

            if (!categoryExists || !roleExists || !allLanguagesValid)
            {
                return new ApiResponseDto(false, "One or more of the selected options are invalid.");
            }

            // Add options that user choose and add row into user selection
            var userSelection = new UserSelection
            {
                UserId = userId,
                CategoryId = dto.CategoryId,
                RoleId = dto.RoleId
            };
            _context.Add(userSelection);

            // Add all choosen programming languages by user
            foreach (var languageId in dto.ProgrammingLanguageIds)
            {
                _context.Add(new UserLanguage
                {
                    UserSelection = userSelection,
                    ProgrammingLanguageId = languageId
                });
            };

            // Add user into queue
            _context.Add(new LobbyQueue
            {
                UserId = userId,
                UserSelection = userSelection,
                Status = "Queued"
            });

            await _context.SaveChangesAsync();

            return new ApiResponseDto(true, "Successfully added to queue.");
        }

        public async Task<QueueTimeDto> GetQueueTimeAsync(int userId)
        {
            var jointedAtTime = await _context.LobbyQueues
                .Where(l => l.UserId == userId)
                .Select(l => l.JoinedAt)
                .FirstOrDefaultAsync();

            TimeSpan queueTime = DateTime.Now - jointedAtTime;
            
            return new QueueTimeDto(true, queueTime);
        }
    }
}

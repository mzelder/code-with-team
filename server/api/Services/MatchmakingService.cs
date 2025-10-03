using api.Data;
using api.Dtos;
using api.Dtos.Matchmaking;
using api.Models;
using api.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;


namespace api.Services
{
    public class MatchmakingService : IMatchmakingService
    {
        private readonly AppDbContext _context;

        public MatchmakingService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponseDto> StartQueueAsync(int userId, ChoosedOptionsDto dto)
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
                CategoryId = dto.CategoryId!.Value,
                RoleId = dto.RoleId!.Value
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
                Status = LobbyQueue.QueueStatus.InQueue
            });

            await _context.SaveChangesAsync();

            return new ApiResponseDto(true, "Successfully added to queue.");
        }
        public async Task<ApiResponseDto> StopQueueAsync(int userId)
        {
            var deletedRows = await _context.LobbyQueues
                .Where(l => l.UserId == userId)
                .ExecuteDeleteAsync();

            if (deletedRows == 0) return new ApiResponseDto(false, "No active queue found for this user");

            return new ApiResponseDto(true, "You left queue successfully");
        }

        public async Task<QueueTimeDto> GetQueueTimeAsync(int userId)
        {
            var jointedAtTime = await _context.LobbyQueues
                .Where(l => l.UserId == userId)
                .Select(l => l.JoinedAt)
                .FirstOrDefaultAsync();

            if (jointedAtTime == null) return new QueueTimeDto(false, "");

            TimeSpan? queueTime = DateTime.Now - jointedAtTime!;
            
            return new QueueTimeDto(true, queueTime.ToString());
        }

        public async Task<ChoosedOptionsDto> GetChoosedOptionsAsync(int userId)
        {
            var choosedOptions = await _context.UserSelections
                .Include(us => us.UserLanguages)
                .Where(us => us.UserId == userId)
                .FirstOrDefaultAsync();

            return new ChoosedOptionsDto
            {
                CategoryId = choosedOptions?.CategoryId,
                RoleId = choosedOptions?.RoleId,
                ProgrammingLanguageIds = choosedOptions?.UserLanguages
                    .Select(ul => ul.ProgrammingLanguageId)
                    .ToList()
            };
        }
    }
}

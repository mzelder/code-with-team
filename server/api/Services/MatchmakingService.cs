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

        public async Task<LobbyStatusDto> TryGetLobbyAsync()
        {
            var usersInQueue = await _context.LobbyQueues
                .Include(lq => lq.UserSelection)
                    .ThenInclude(us => us.Category)
                .Include(lq => lq.UserSelection)
                    .ThenInclude(us => us.Role)
                .Include(lq => lq.User)
                .ToListAsync();
            var groupedByCategory = usersInQueue.GroupBy(u => u.UserSelection.CategoryId);

            Lobby? lobby = null;
            List<LobbyMember>? potentialLobbyMembers = new List<LobbyMember>();

            foreach (var group in groupedByCategory)
            {
                potentialLobbyMembers = new List<LobbyMember>();

                foreach (var user in group)
                {
                    if (potentialLobbyMembers.Any(plm => plm.User == user.User)) continue;
                    if (potentialLobbyMembers.Any(plm => plm.UserSelection.Role == user.UserSelection.Role)) continue;

                    potentialLobbyMembers.Add(new LobbyMember
                    {
                        UserId = user.UserId,
                        User = user.User,
                        UserSelectionId = user.UserSelectionId,
                        UserSelection = user.UserSelection,
                        JoinedAt = DateTime.UtcNow
                    });
                }

                if (potentialLobbyMembers.Count >= 4)
                {
                    lobby = new Lobby
                    {
                        Status = "Active",
                        CreatedAt = DateTime.Now,
                    };
                    _context.Add(lobby);

                    foreach (var member in potentialLobbyMembers)
                    {
                        _context.Add(new LobbyMember
                        {
                            Lobby = lobby,
                            User = member.User,
                            UserSelection = member.UserSelection,
                            JoinedAt = DateTime.Now
                        });
                    }

                    var userIdsToDelete = potentialLobbyMembers.Select(pm => pm.UserId).ToList();
                    _context.LobbyQueues.RemoveRange(
                        _context.LobbyQueues.Where(lq => userIdsToDelete.Contains(lq.UserId))
                    );

                    await _context.SaveChangesAsync();

                    return new LobbyStatusDto
                    {
                        Found = true,
                        LobbyId = lobby.Id,
                        Members = potentialLobbyMembers.Select(pm => new LobbyMemberDto
                        {
                            Name = pm.User.Username,
                            Category = pm.UserSelection.Category.Name,
                            Role = pm.UserSelection.Role.Name
                        }).ToList()
                    };
                }
            }

            return new LobbyStatusDto
            {
                Found = false,
                LobbyId = null,
                Members = null
            };
        }
    }
}

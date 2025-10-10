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
        private const int _lobbySize = 4;

        public MatchmakingService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponseDto> StartQueueAsync(int userId, ChoosedOptionsDto dto)
        {
            var userInLobby = await _context.LobbyMembers
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
            }
            ;

            // Add user into queue
            _context.Add(new LobbyMember
            {
                UserId = userId,
                UserSelection = userSelection,
                Status = LobbyMember.QueueStatus.InQueue
            });

            await _context.SaveChangesAsync();

            return new ApiResponseDto(true, "Successfully added to queue.");
        }
        public async Task<ApiResponseDto> StopQueueAsync(int userId)
        {
            var deletedRows = await _context.LobbyMembers
                .Where(l => l.UserId == userId)
                .ExecuteDeleteAsync();

            if (deletedRows == 0) return new ApiResponseDto(false, "No active queue found for this user");

            return new ApiResponseDto(true, "You left queue successfully");
        }

        public async Task<QueueTimeDto> GetQueueTimeAsync(int userId)
        {
            var jointedAtTime = await _context.LobbyMembers
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

        public async Task<LobbyStatusDto> GetLobbyStatusAsync(int userId, CancellationToken ct = default)
        {
            var lobbyId = await _context.LobbyMembers
                 .Where(lm => lm.UserId == userId)
                 .Select(lm => (int?)lm.LobbyId)
                 .FirstOrDefaultAsync(ct);

            if (lobbyId == null)
            {
                return new LobbyStatusDto
                {
                    Found = false,
                    LobbyId = null,
                    Members = null
                };
            }

            var members = await _context.LobbyMembers
                .Include(lm => lm.User)
                .Include(lm => lm.UserSelection)
                    .ThenInclude(us => us.Category)
                .Include(lm => lm.UserSelection)
                    .ThenInclude(us => us.Role)
                .Where(lm => lm.LobbyId == lobbyId.Value)
                .ToListAsync(ct);

            return new LobbyStatusDto
            {
                Found = true,
                LobbyId = lobbyId,
                Members = members.Select(m => new LobbyMemberDto
                {
                    Name = m.User.Username,
                    Category = m.UserSelection.Category.Name,
                    Role = m.UserSelection.Role.Name
                }).ToList()
            };
        }

        public async Task FormLobbiesAsync(CancellationToken ct = default)
        {
            var usersInQueue = await _context.LobbyMembers
                .Include(lq => lq.UserSelection)
                    .ThenInclude(us => us.Category)
                .Include(lq => lq.UserSelection)
                    .ThenInclude(us => us.Role)
                .Include(lq => lq.User)
                .ToListAsync(ct);

            if (usersInQueue.Count == 0) return;

            var groupedByCategory = usersInQueue.GroupBy(u => u.UserSelection.CategoryId);
            foreach (var group in groupedByCategory)
            {
                var potentialLobbyMembers = new List<LobbyMember>();
                foreach (var user in group)
                {
                    if (potentialLobbyMembers.Any(plm => plm.User == user.User)) continue;
                    if (potentialLobbyMembers.Any(plm => plm.UserSelection.Role == user.UserSelection.Role)) continue;

                    potentialLobbyMembers.Add(user);

                    if (potentialLobbyMembers.Count == 4) break;
                }

                if (potentialLobbyMembers.Count == 4)
                {
                    var lobby = new Lobby
                    {
                        Status = "Active",
                        CreatedAt = DateTime.Now,
                    };
                    _context.Lobbies.Add(lobby);

                    foreach (var member in potentialLobbyMembers)
                    {
                        member.Lobby = lobby;
                        member.Status = LobbyMember.QueueStatus.FoundLobby;
                    }

                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}

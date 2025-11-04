using api.Data;
using api.Dtos.TaskProgress;
using api.Models.Tasks;
using api.Services.Interfaces;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class TaskProgressService : ITaskProgressService
    {
        private readonly AppDbContext _context;
        private readonly IGithubBotService _githubBotService;

        public TaskProgressService(AppDbContext context, IGithubBotService githubBotService)
        {
            _context = context;
            _githubBotService = githubBotService;
        }

        public async Task<IEnumerable<UserTask>> GetUserTaskProgressAsync(int userId)
        {
            var tasks = await _context.UserTasks
                .Include(ut => ut.UserTaskProgress)
                    .ThenInclude(utp => utp.LobbyMember)
                .Where(ut => ut.UserTaskProgress.LobbyMember.UserId == userId)
                .ToListAsync();

            if (!tasks.Any())
            {
                throw new InvalidOperationException($"Task progress for user ID {userId} does not exist.");
            }

            return tasks;
        }

        public async Task<IEnumerable<TeamTask>> GetTeamTaskProgressAsync(int userId)
        {
            var lobbyId = await _context.LobbyMembers
                .Where(lm => lm.UserId == userId)
                .Select(lm => lm.LobbyId)
                .FirstOrDefaultAsync();

            if (lobbyId == null)
            {
                throw new InvalidOperationException($"User {userId} is not a member of any lobby.");
            }

            var tasks = await _context.TeamTasks
                .Include(tt => tt.TeamTaskProgress)
                    .ThenInclude(ttp => ttp.Lobby)
                .Where(tt => tt.TeamTaskProgress.LobbyId == lobbyId)
                .ToListAsync();

            if (!tasks.Any())
            {
                throw new InvalidOperationException($"Team task progress for lobby ID {lobbyId} does not exist.");
            }

            var createdIssuesTask = tasks
                .FirstOrDefault(t => t.Name == "Break down the tasks"); // change it to enumerate??

            if (createdIssuesTask != null && !createdIssuesTask.IsCompleted)
            {
                await UpdateCreatedIssuesAsync(lobbyId.Value, createdIssuesTask);
            }

            return tasks;
        }

        private async Task UpdateCreatedIssuesAsync(int lobbyId, TeamTask createdIssuesTask)
        {
            var lobby = await _context.Lobbies
                .FirstOrDefaultAsync(l => l.Id == lobbyId);

            if (lobby == null) return;

            var repoName = lobby.RepositoryUrl.Split("/")[^1];
            var repo = await _githubBotService.GetRepositoryByNameAsync(repoName);
            var issuesCount = await _githubBotService.GetIssueCountAsync(repo);
            
            if (issuesCount < 5) return;

            createdIssuesTask.IsCompleted = true;
            await _context.SaveChangesAsync();
        }
    }
}

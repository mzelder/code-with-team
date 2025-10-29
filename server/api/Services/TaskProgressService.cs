using api.Data;
using api.Dtos.TaskProgress;
using api.Models;
using api.Services.Interfaces;
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

        public async Task<UserTaskProgress> GetUserTaskProgressAsync(int userId)
        {
            var taskProgress = await _context.UserTaskProgresses
                .FirstOrDefaultAsync(utp => utp.LobbyMember.UserId == userId);
            if (taskProgress == null)
            {
                throw new InvalidOperationException($"Task progress for user ID {userId} does not exist.");
            }
            return taskProgress;
        }

        public async Task<TeamTaskProgress> GetTeamTaskProgressAsync(int userId)
        {
            var lobbyId = await _context.LobbyMembers
                .Where(lm => lm.UserId == userId)
                .Select(lm => lm.LobbyId)
                .FirstOrDefaultAsync();

            var teamTaskProgress = await _context.TeamTaskProgresses
                .FirstOrDefaultAsync(ttp => ttp.LobbyId == lobbyId);
            
            if (teamTaskProgress == null || lobbyId == null)
            {
                throw new InvalidOperationException($"Team task progress for lobby ID {lobbyId} does not exist.");
            }

            if (!teamTaskProgress.CreatedIssues)
            {
                await UpdateCreatedIssuesAsync(lobbyId.Value);
            }

            return teamTaskProgress;
        }

        private async Task UpdateCreatedIssuesAsync(int lobbyId)
        {
            var lobby = await _context.Lobbies
                .Where(l => l.Id == lobbyId)
                .FirstOrDefaultAsync();

            var repoName = lobby.RepositoryUrl.Split("/")[^1];
            var repo = await _githubBotService.GetRepositoryByNameAsync(repoName);
            var issuesCount = await _githubBotService.GetIssueCountAsync(repo);
            
            if (issuesCount < 5) return;

            lobby.TeamTaskProgress.CreatedIssues = true;
            await _context.SaveChangesAsync();
        }
    }
}

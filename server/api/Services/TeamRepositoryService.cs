using api.Data;
using api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class TeamRepositoryService : ITeamRepositoryService
    {
        private readonly IGithubBotService _githubBotService;
        private readonly IGithubUserService _githubUserService;
        private readonly AppDbContext _context;
        private readonly string _organizationName;


        public TeamRepositoryService(
            IGithubBotService githubBotService,
            IGithubUserService githubUserService,
            AppDbContext context,
            IConfiguration configuration
        )
        {
            _githubBotService = githubBotService;
            _githubUserService = githubUserService;
            _context = context;
            _organizationName = configuration["Github:OrganizationName"] 
                ?? throw new InvalidOperationException("Github organization name is not configured.");
        }

        public async Task SetupTeamRepositoryAsync(int lobbyId, CancellationToken cancellationToken = default)
        {
            var lobby = await _context.Lobbies.FindAsync(lobbyId);
            if (lobby == null)
            {
                throw new InvalidOperationException($"Lobby with ID {lobbyId} does not exist.");
            }

            var lobbyMembers = await _context.LobbyMembers
                .Where(lm => lm.LobbyId == lobbyId)
                .ToListAsync(cancellationToken);
            if (!lobbyMembers.Any())
            {
                throw new InvalidOperationException($"Lobby with ID {lobbyId} has no members.");
            }

            var users = await _context.Users
                .Where(u => lobbyMembers.Select(lm => lm.UserId).Contains(u.Id))
                .ToListAsync(cancellationToken);
            if (!users.Any())
            {
                throw new InvalidOperationException($"No users found for lobby ID {lobbyId}.");
            }

            try
            {
                // Create repository from template
                var repository = await _githubBotService.CreateRepositoryFromTemplateAsync(
                    _organizationName,
                    GenerateRepositoryName()
                );
                await Task.Delay(5000, cancellationToken);

                // Set branch protection rules
                await _githubBotService.SetBranchRulesAsync(_organizationName, repository.Name);

                // Create project for repository
                var project = await _githubBotService.CreateProjectAsync(
                    repository,
                    _organizationName,
                    repository.Name
                );

                // Add collaborators to reposisotry and project
                foreach (var user in users)
                {
                    await _githubBotService.AddColaboratorToRepoAsync(
                        _organizationName,
                        repository.Name,
                        user.Username
                    );

                    await _githubBotService.AddColaboratorToProjectAsync(
                        repository,
                        project,
                        _organizationName
                    );

                    await _githubUserService.AcceptRepositoryInvitationAsync(
                        _organizationName,
                        user.Id
                    );
                }

                // Save repository url to lobby element
                lobby.RepositoryUrl = repository.HtmlUrl;
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to set up repository for lobby ID {lobbyId}: {ex.Message}");
            }
        }

        private string GenerateRepositoryName()
        {
            var r = new Random();
            string digits = ""; 
            for (int i = 0; i < 5; i++) digits += r.Next(0,10).ToString();
            return $"{digits}-flask-js";
        }
    }
}

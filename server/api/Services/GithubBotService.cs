using api.Services.Interfaces;
using Octokit;

namespace api.Services
{
    public class GithubBotService : IGithubBotService
    {
        private readonly IGithubAppService _githubAppService;
        private readonly IConfiguration _configuration;

        public GithubBotService(IGithubAppService githubAppService, IConfiguration configuration)
        {
            _githubAppService = githubAppService;
            _configuration = configuration;
        }

        public async Task<Repository> CreateRepositoryAsync(string organizationName, string repoName)
        {
            var client = await _githubAppService.GetInstallationAccessClientAsync(organizationName);

            var newRepo = new NewRepository(repoName)
            {
                AutoInit = true,
            };
            return await client.Repository.Create(organizationName, newRepo);
        }

        public async Task AddColaboratorAsync(string organizationName, string repoName, string collaboratorUsername)
        {
            var client = await _githubAppService.GetInstallationAccessClientAsync(organizationName);
            await client.Repository.Collaborator.Add(organizationName, repoName, collaboratorUsername);
        }

        public async Task AcceptInvitationOnBehalfOfUserAsync(string organizationName, string userOAuthToken, int invitationId)
        {
            var userClient = new GitHubClient(new ProductHeaderValue(organizationName))
            {
                Credentials = new Credentials(userOAuthToken)
            };

            await userClient.Repository.Invitation.Accept(invitationId);
        }
    }
}

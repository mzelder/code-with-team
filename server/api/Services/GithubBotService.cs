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

            var newRepo = new NewRepository(repoName);
            return await client.Repository.Create(organizationName, newRepo);
        }
    }
}

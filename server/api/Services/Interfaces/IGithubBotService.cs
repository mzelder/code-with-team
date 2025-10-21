using Octokit;

namespace api.Services.Interfaces
{
    public interface IGithubBotService
    {
        Task<Repository> CreateRepositoryAsync(string organizationName, string repoName);
    }
}

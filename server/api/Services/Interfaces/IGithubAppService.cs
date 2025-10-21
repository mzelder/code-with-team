using Octokit;

namespace api.Services.Interfaces
{
    public interface IGithubAppService
    {
        Task<GitHubClient> GetInstallationAccessClientAsync(string organizationName);
        Task<int> GetInstalationAsync(string organizationName, string jwt);
    }
}

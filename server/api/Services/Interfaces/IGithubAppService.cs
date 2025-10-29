using Octokit;
using GraphQL = Octokit.GraphQL;

namespace api.Services.Interfaces
{
    public interface IGithubAppService
    {
        Task<GitHubClient> GetInstallationAccessClientAsync();
        Task<int> GetInstalationAsync(string jwt);
        Task<GraphQL.Connection> GetGraphQLConnection();
    }
}

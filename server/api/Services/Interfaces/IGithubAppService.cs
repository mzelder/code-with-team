using Octokit;
using GraphQL = Octokit.GraphQL;

namespace api.Services.Interfaces
{
    public interface IGithubAppService
    {
        Task<GitHubClient> GetInstallationAccessClientAsync(string organizationName);
        Task<int> GetInstalationAsync(string organizationName, string jwt);
        Task<GraphQL.Connection> GetGraphQLConnection(string organizationName);
    }
}

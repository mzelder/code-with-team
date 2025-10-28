using api.Dtos.Github;
using Octokit;

namespace api.Services.Interfaces
{
    public interface IGithubBotService
    {
        Task<Repository> CreateRepositoryAsync(string organizationName, string repoName);
        Task<Repository> CreateRepositoryFromTemplateAsync(string organizationName, string repoName);
        Task AddColaboratorToRepoAsync(string organizationName, string repoName, string collaboratorUsername);
        Task SetBranchRulesAsync(string organizationName, string repoName, string branch = "main");
        Task<ProjectResult> CreateProjectAsync(Repository repository, string organizationName, string projectName);
        Task AddColaboratorToProjectAsync(Repository repository, ProjectResult project, string organizationName);
    }
}

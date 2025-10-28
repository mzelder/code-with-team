using api.Dtos.Github;
using Octokit;

namespace api.Services.Interfaces
{
    public interface IGithubBotService
    {
        Task<Repository> CreateRepositoryAsync(string repoName);
        Task<Repository> CreateRepositoryFromTemplateAsync(string repoName);
        Task AddColaboratorToRepoAsync(string repoName, string collaboratorUsername);
        Task SetBranchRulesAsync(string repoName, string branch = "main");
        Task<ProjectResult> CreateProjectAsync(Repository repository, string projectName);
        Task AddColaboratorToProjectAsync(ProjectResult project, int userId);
    }
}

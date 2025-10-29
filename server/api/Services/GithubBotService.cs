using api.Dtos.Github;
using api.Services.Interfaces;
using Microsoft.Identity.Client;
using Octokit;
using Octokit.GraphQL;
using GraphQL = Octokit.GraphQL;

namespace api.Services
{
    public class GithubBotService : IGithubBotService
    {
        private readonly IGithubAppService _githubAppService;
        private readonly IGithubUserService _githubUserService;
        private readonly IConfiguration _configuration;
        private readonly string _organizationName;

        public GithubBotService(IGithubAppService githubAppService, 
            IGithubUserService githubUserService,
            IConfiguration configuration)
        {
            _githubAppService = githubAppService;
            _githubUserService = githubUserService;
            _configuration = configuration;
            _organizationName = configuration["Github:OrganizationName"];
        }

        public async Task<Repository> CreateRepositoryAsync(string repoName)
        {
            var client = await _githubAppService.GetInstallationAccessClientAsync();

            var newRepo = new NewRepository(repoName)
            {
                AutoInit = true,
            };
            return await client.Repository.Create(_organizationName, newRepo);
        }

        public async Task<Repository> CreateRepositoryFromTemplateAsync(string repoName)
        {
            var templateRepoName = _configuration["Github:TemplateRepoName"];
            if (string.IsNullOrEmpty(templateRepoName))
            {
                throw new InvalidOperationException("Template repository name is not configured.");
            }

            var client = await _githubAppService.GetInstallationAccessClientAsync();
            var newRepo = new NewRepositoryFromTemplate(repoName)
            {
                Owner = _organizationName
            };

            return await client.Repository.Generate(_organizationName, templateRepoName, newRepo);
        }

        public async Task AddColaboratorToRepoAsync(string repoName, string collaboratorUsername)
        {
            var client = await _githubAppService.GetInstallationAccessClientAsync();
            var permision = new CollaboratorRequest("push");
            await client.Repository.Collaborator.Add(_organizationName, repoName, collaboratorUsername, permision);
        }

        public async Task SetBranchRulesAsync(string repoName, string branch = "main")
        {
            var client = await _githubAppService.GetInstallationAccessClientAsync();
            var pullRequestReviews = new BranchProtectionRequiredReviewsUpdate(
                requiredApprovingReviewCount: 2,
                dismissStaleReviews: true,
                requireCodeOwnerReviews: false,
                requireLastPushApproval: false,
                dismissalRestrictions: new BranchProtectionRequiredReviewsDismissalRestrictionsUpdate(false)
            );
            var branchProtection = new BranchProtectionSettingsUpdate(
                requiredStatusChecks: null,
                requiredPullRequestReviews: pullRequestReviews,
                restrictions: null,
                requiredLinearHistory: false,
                allowForcePushes: null,
                allowDeletions: false,
                blockCreations: false,
                requiredConversationResolution: true,
                requiredSignatures: false,
                enforceAdmins: false
            );

            await client.Repository.Branch.UpdateBranchProtection(_organizationName, repoName, branch, branchProtection);
        }

        public async Task<ProjectResult> CreateProjectAsync(Repository repository, string projectName)
        {
            var connection = await _githubAppService.GetGraphQLConnection();
            var mutation = new Mutation()
                .CreateProjectV2(new GraphQL.Model.CreateProjectV2Input
                {
                    OwnerId = new ID(repository.Owner.NodeId),
                    Title = projectName,
                    RepositoryId = new ID(repository.NodeId)
                })
                .Select(p => new
                {
                    p.ProjectV2.Id,
                    p.ProjectV2.Url
                }).Compile();   

            var result = await connection.Run(mutation);
            
            return new ProjectResult
            {
                Id = result.Id,
                Url = result.Url.ToString()
            };
        }

        public async Task AddColaboratorToProjectAsync(ProjectResult project, int userId)
        {
            var connection = await _githubAppService.GetGraphQLConnection();

            var projectCollaborator = new GraphQL.Model.ProjectV2Collaborator
            {
                UserId = new ID(await _githubUserService.GetUserNodeIDAsync(userId)),
                Role = GraphQL.Model.ProjectV2Roles.Writer
            };
            
            var mutation = new Mutation()
                .UpdateProjectV2Collaborators(new GraphQL.Model.UpdateProjectV2CollaboratorsInput
                {
                    ProjectId = project.Id,
                    Collaborators = [projectCollaborator]
                })
                .Select(p => p.ClientMutationId);

            var result = await connection.Run(mutation);
        }

        public async Task<int> GetIssueCountAsync(Repository repository)
        {
            var client = await _githubAppService.GetInstallationAccessClientAsync();
            var issues = await client.Issue.GetAllForRepository(repository.Id);
            return issues.Count;
        }
        public async Task<Repository> GetRepositoryByNameAsync(string repoName)
        {
            var client = await _githubAppService.GetInstallationAccessClientAsync();
            return await client.Repository.Get(_organizationName, repoName);
        }
    }
}

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

        public async Task<Repository> CreateRepositoryFromTemplateAsync(string organizationName, string repoName)
        {
            var templateRepoName = _configuration["Github:TemplateRepoName"];
            if (string.IsNullOrEmpty(templateRepoName))
            {
                throw new InvalidOperationException("Template repository name is not configured.");
            }

            var client = await _githubAppService.GetInstallationAccessClientAsync(organizationName);
            var newRepo = new NewRepositoryFromTemplate(repoName)
            {
                Owner = organizationName
            };

            return await client.Repository.Generate(organizationName, templateRepoName, newRepo);
        }

        public async Task AddColaboratorToRepoAsync(string organizationName, string repoName, string collaboratorUsername)
        {
            var client = await _githubAppService.GetInstallationAccessClientAsync(organizationName);
            var permision = new CollaboratorRequest("push");
            await client.Repository.Collaborator.Add(organizationName, repoName, collaboratorUsername, permision);
        }

        public async Task SetBranchRulesAsync(string organizationName, string repoName, string branch = "main")
        {
            var client = await _githubAppService.GetInstallationAccessClientAsync(organizationName);
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

            await client.Repository.Branch.UpdateBranchProtection(organizationName, repoName, branch, branchProtection);
        }

        public async Task<ProjectResult> CreateProjectAsync(Repository repository, string organizationName, string projectName)
        {
            var connection = await _githubAppService.GetGraphQLConnection(organizationName);
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

        public async Task AddColaboratorToProjectAsync(Repository repository, ProjectResult project, string organizationName)
        {
            var client = await _githubAppService.GetInstallationAccessClientAsync(organizationName);
            var connection = await _githubAppService.GetGraphQLConnection(organizationName);

            var reopCollaborators = await client.Repository.Collaborator.GetAll(repository.Owner.Login, repository.Name);

            var role = GraphQL.Model.ProjectV2Roles.Writer;
            var projectCollaborators = new List<GraphQL.Model.ProjectV2Collaborator>();
            
            foreach (var collaborator in reopCollaborators)
            {
                var userId = new ID(collaborator.NodeId);
                projectCollaborators.Add(new GraphQL.Model.ProjectV2Collaborator
                {
                    UserId = userId,
                    Role = role
                });
            }

            var mutation = new Mutation()
                .UpdateProjectV2Collaborators(new GraphQL.Model.UpdateProjectV2CollaboratorsInput
                {
                    ProjectId = project.Id,
                    Collaborators = projectCollaborators
                })
                .Select(p => p.ClientMutationId);

            var result = await connection.Run(mutation);
        }
    }
}

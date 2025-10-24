using api.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Query.Internal;
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

        public async Task AddColaboratorAsync(string organizationName, string repoName, string collaboratorUsername)
        {
            var client = await _githubAppService.GetInstallationAccessClientAsync(organizationName);
            var permision = new CollaboratorRequest("push");
            await client.Repository.Collaborator.Add(organizationName, repoName, collaboratorUsername, permision);
        }

        public async Task SetBranchRulesAsync(string organizationName, string repoName, string branch="main")
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
    }
}

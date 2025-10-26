using api.Data;
using api.Services.Interfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration.UserSecrets;
using Octokit;

namespace api.Services
{
    public class GithubUserService : IGithubUserService
    {
        private readonly AppDbContext _context;
        private readonly IDataProtector _protector;

        public GithubUserService(IDataProtectionProvider dataProtectorProvider, AppDbContext context)
        {
            _protector = dataProtectorProvider.CreateProtector("GitHubTokenProtector");
            _context = context;
        }

        private async Task<GitHubClient> GetUserClientAsync(string organizationName, int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            var decryptedToken = _protector.Unprotect(user.GithubToken);

            var userClient = new GitHubClient(new ProductHeaderValue(organizationName))
            {
                Credentials = new Credentials(decryptedToken)
            };
            return userClient;
        }

        public async Task AcceptRepositoryInvitationAsync(string organizationName, int userId)
        {
            var client = await GetUserClientAsync(organizationName, userId);
            var invitations = await client.Repository.Invitation.GetAllForCurrent();
            var orgInvitations = invitations
                .Where(i => i.Repository.Owner.Login.Equals(organizationName, StringComparison.OrdinalIgnoreCase))
                .ToList();

            foreach (var invitation in orgInvitations)
            {
                await client.Repository.Invitation.Accept(invitation.Id);
            }
        }
    }
}

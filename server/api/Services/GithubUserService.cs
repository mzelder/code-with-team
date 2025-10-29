using api.Data;
using api.Services.Interfaces;
using Microsoft.AspNetCore.DataProtection;
using Octokit;

namespace api.Services
{
    public class GithubUserService : IGithubUserService
    {
        private readonly AppDbContext _context;
        private readonly IDataProtector _protector;
        private readonly string _organizationName;

        public GithubUserService(IDataProtectionProvider dataProtectorProvider, 
            AppDbContext context,
            IConfiguration configuration)
        {
            _protector = dataProtectorProvider.CreateProtector("GitHubTokenProtector");
            _context = context;
            _organizationName = configuration["Github:OrganizationName"];
        }

        private async Task<GitHubClient> GetUserClientAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            var decryptedToken = _protector.Unprotect(user.GithubToken);

            var userClient = new GitHubClient(new ProductHeaderValue(_organizationName))
            {
                Credentials = new Credentials(decryptedToken)
            };
            return userClient;
        }

        public async Task<string> GetUserNodeIDAsync(int userId)
        {
            var client = await GetUserClientAsync(userId);
            var user = await client.User.Current();
            return user.NodeId;
        }
        
        public async Task AcceptRepositoryInvitationAsync(int userId)
        {
            var client = await GetUserClientAsync(userId);
            var invitations = await client.Repository.Invitation.GetAllForCurrent();
            var orgInvitations = invitations
                .Where(i => i.Repository.Owner.Login.Equals(_organizationName, StringComparison.OrdinalIgnoreCase))
                .ToList();

            foreach (var invitation in orgInvitations)
            {
                await client.Repository.Invitation.Accept(invitation.Id);
            }
        }
    }
}

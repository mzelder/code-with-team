using Octokit;
using System.Runtime.CompilerServices;

namespace api.Services.Interfaces
{
    public interface IGithubUserService
    {
        Task AcceptRepositoryInvitationAsync(string organizationName, int userId);
    }
}

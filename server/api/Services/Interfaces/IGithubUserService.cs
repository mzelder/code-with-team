using Octokit;
using System.Runtime.CompilerServices;

namespace api.Services.Interfaces
{
    public interface IGithubUserService
    {
        Task AcceptRepositoryInvitationAsync(int userId);
        Task<string> GetUserNodeIDAsync(int userId);
    }
}

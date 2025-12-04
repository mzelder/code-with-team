using api.Dtos.TaskProgress;
using api.Models;
using api.Models.Tasks;

namespace api.Services.Interfaces
{
    public interface ITaskProgressService
    {
        Task<IEnumerable<UserTask>> GetUserTaskProgressAsync(int userId);
        Task<IEnumerable<TeamTask>> GetTeamTaskProgressAsync(int userId);
        
        Task UpdateCreatedIssuesAsync(int lobbyId, TeamTask createdIssuesTask);

        Task UpdateTeamTaskAsync(int lobbyId, string taskName);
        Task UpdateUserTaskAsync(int userId, string taskName);

        Task UpdateFinishAsync(int userId);
    }
}

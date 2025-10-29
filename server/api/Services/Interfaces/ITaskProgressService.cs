using api.Dtos.TaskProgress;
using api.Models;

namespace api.Services.Interfaces
{
    public interface ITaskProgressService
    {
        Task<UserTaskProgress> GetUserTaskProgressAsync(int userId);
        Task<TeamTaskProgress> GetTeamTaskProgressAsync(int userId);
    }
}

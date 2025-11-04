using api.Dtos.TaskProgress;
using api.Models.Tasks;

namespace api.Services.Interfaces
{
    public interface ITaskProgressService
    {
        Task<IEnumerable<UserTask>> GetUserTaskProgressAsync(int userId);
        Task<IEnumerable<TeamTask>> GetTeamTaskProgressAsync(int userId);
    }
}

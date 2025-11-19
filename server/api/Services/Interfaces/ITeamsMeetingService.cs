using Microsoft.Graph.Models;

namespace api.Services.Interfaces
{
    public interface ITeamsMeetingService
    {
        Task<OnlineMeeting> CreateOnlineMeetingAsync(DateTimeOffset startDateTime);
    }
}

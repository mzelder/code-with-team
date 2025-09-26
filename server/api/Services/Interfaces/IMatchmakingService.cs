using api.Dtos;
using api.Dtos.Matchmaking;

namespace api.Services.Interfaces
{
    public interface IMatchmakingService
    {
        Task<ApiResponseDto> StartQueueAsync(int userId, ChoosedOptionsDto dto);
        Task<ApiResponseDto> StopQueueAsync(int userId);
        Task<QueueTimeDto> GetQueueTimeAsync(int userId);
        Task<ChoosedOptionsDto> GetChoosedOptionsAsync(int userId);
    }
}

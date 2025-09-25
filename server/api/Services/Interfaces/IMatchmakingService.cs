using api.Dtos;
using api.Dtos.Matchmaking;

namespace api.Services.Interfaces
{
    public interface IMatchmakingService
    {
        Task<ApiResponseDto> StartQueueAsync(int userId, MatchmakingRequestDto dto);
        Task<QueueTimeDto> GetQueueTimeAsync(int userId);
    }
}

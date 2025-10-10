using api.Dtos;
using api.Dtos.Matchmaking;
using api.Models;

namespace api.Services.Interfaces
{
    public interface IMatchmakingService
    {
        Task<ApiResponseDto> StartQueueAsync(int userId, ChoosedOptionsDto dto);
        Task<ApiResponseDto> StopQueueAsync(int userId);
        Task<QueueTimeDto> GetQueueTimeAsync(int userId);
        Task<ChoosedOptionsDto> GetChoosedOptionsAsync(int userId);
        Task<LobbyStatusDto> GetLobbyStatusAsync(int userId, CancellationToken ct = default);
        Task FormLobbiesAsync(CancellationToken ct = default);
    }
}

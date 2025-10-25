namespace api.Services.Interfaces
{
    public interface ITeamRepositoryService
    {
        Task SetupTeamRepositoryAsync(int lobbyId, CancellationToken cancellationToken = default);
    }
}

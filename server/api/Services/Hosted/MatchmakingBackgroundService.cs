using api.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Client;

namespace api.Services.Hosted
{
    public class MatchmakingBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private const int IntervalMs = 2000;

        public MatchmakingBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var matchmakingService = scope.ServiceProvider.GetRequiredService<IMatchmakingService>();
                    var teamRepositoryService = scope.ServiceProvider.GetRequiredService<ITeamRepositoryService>();

                    await matchmakingService.FormLobbiesAsync(stoppingToken);

                    var lobbyWithoutUrl = await matchmakingService.GetFirstLobbyWithoutRepositoryUrl(stoppingToken);
                    if (lobbyWithoutUrl != null)
                    {
                        await teamRepositoryService.SetupTeamRepositoryAsync(lobbyWithoutUrl.Id ,stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Background task error: " + ex.Message);
                }

                await Task.Delay(IntervalMs, stoppingToken);
            }
        }
    }
}
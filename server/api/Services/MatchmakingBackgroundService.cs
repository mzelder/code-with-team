using api.Services.Interfaces;
using Microsoft.Extensions.Hosting;

namespace api.Services.Hosted
{
    public class MatchmakingBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private const int IntervalMs = 5000;

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
                    var matchmaking = scope.ServiceProvider.GetRequiredService<IMatchmakingService>();
                    await matchmaking.FormLobbiesAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                await Task.Delay(IntervalMs, stoppingToken);
            }
        }
    }
}
using api.Data;
using api.Models;
using api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.Services.Hosted
{
    public class CodeReviewBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private const int IntervalMs = 10_000;

        public CodeReviewBackgroundService(IServiceProvider serviceProvider)
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
                    var codeReviewService = scope.ServiceProvider.GetRequiredService<ICodeReviewService>();
                    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    var finishedLobbies = await context.Lobbies
                        .Where(l => l.Status == LobbyStatus.Finished && l.AiSummary == null)
                        .ToListAsync(stoppingToken);

                    foreach (var lobby in finishedLobbies)
                    {
                        await codeReviewService.GenerateLobbySummaryAsync(lobby, stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in CodeReviewBackgroundService: {ex.Message}");
                }

                await Task.Delay(IntervalMs, stoppingToken);
            }
        }
    }
}
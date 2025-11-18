using api.Data;
using api.Models.Meetings;
using api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace api.Services.Hosted
{
    public class MeetingSchedulerBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private const int IntervalMs = 60_000;

        public MeetingSchedulerBackgroundService(IServiceProvider serviceProvider)
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
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var teams = scope.ServiceProvider.GetRequiredService<ITeamsMeetingService>();

                    var proposals = await db.MeetingProposals
                        .Where(p => p.Status == MeetingProposalStatus.Accepted && p.Meeting == null)
                        .OrderBy(p => p.MeetingTime)
                        .Take(10)
                        .ToListAsync(stoppingToken);

                    foreach (var p in proposals)
                    {
                        try
                        {
                            var startUtc = DateTime.SpecifyKind(p.MeetingTime, DateTimeKind.Utc);
                            var online = await teams.CreateOnlineMeetingAsync(startUtc);
                            var link = online.JoinWebUrl;

                            db.Meetings.Add(new Meeting
                            {
                                MeetingProposalId = p.Id,
                                TeamsMeetingLink = link
                            });

                            await db.SaveChangesAsync(stoppingToken);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("MeetingScheduler error: " + ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("MeetingScheduler loop error: " + ex.Message);
                }

                await Task.Delay(IntervalMs, stoppingToken);
            }
        }
    }
}
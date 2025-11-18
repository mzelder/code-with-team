using api.Services.Interfaces;
using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Kiota.Abstractions;

namespace api.Services
{
    public class TeamsMeetingService : ITeamsMeetingService
    {
        private readonly GraphServiceClient _graph;
        private readonly IConfiguration _configuration;
        private readonly string _userId;

        public TeamsMeetingService(IConfiguration configuration)
        {
            _configuration = configuration;
            _userId = configuration["Azure:UserId"];
            var tenantId = _configuration["Azure:TenantId"];
            var clientId = _configuration["Azure:ClientId"];
            var clientSecret = _configuration["Azure:ClientSecret"];
            var credentials = new ClientSecretCredential(tenantId, clientId, clientSecret);
            _graph = new GraphServiceClient(credentials);
        }

        public async Task<OnlineMeeting> CreateOnlineMeetingAsync(DateTimeOffset startDateTime)
        {
            var onlineMeeting = new OnlineMeeting
            {
                StartDateTime = startDateTime,
                EndDateTime = startDateTime.AddHours(1),
                Subject = "Code with Team Meeting"
            };
            return await _graph.Users[_userId].OnlineMeetings.PostAsync(onlineMeeting);
        }
    }
}

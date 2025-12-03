using api.Data;
using api.Models;
using api.Models.Mentors;
using api.Services.Interfaces;

namespace api.Services
{
    public class CodeReviewService : ICodeReviewService
    {
        private readonly AppDbContext _context;
        private readonly IGreptileReviewService _greptile;

        public CodeReviewService(AppDbContext context, IGreptileReviewService greptile)
        {
            _context = context;
            _greptile = greptile;
        }   

        public async Task GenerateLobbySummaryAsync(Lobby lobby, CancellationToken ct = default)
        {
            if (lobby == null)
                throw new ArgumentNullException(nameof(lobby));

            var repoName = GetRepoName(lobby.RepositoryUrl);
            var summary = await _greptile.ReviewCodeAsync(repoName);

            var ai = new AiSummary
            {
                Lobby = lobby,
                SummaryText = summary,
                GeneratedAt = DateTime.UtcNow
            };
            _context.AiSummaries.Add(ai);
            await _context.SaveChangesAsync();
        }

        private static string GetRepoName(string repoUrl)
        {
            if (string.IsNullOrWhiteSpace(repoUrl))
                throw new ArgumentException("Repository URL cannot be null or empty");

            var segments = repoUrl.Split("/");
            var repoOwner = segments[^2];
            var repoName = segments[^1];
            return $"{repoOwner}/{repoName}";
        }
    }
}

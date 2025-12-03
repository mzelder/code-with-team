using api.Services.Interfaces;
using Azure;
using Microsoft.Extensions.FileProviders;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text.Json;

namespace api.Services
{
    public class GreptileReviewService : IGreptileReviewService
    {
        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _env;

        public GreptileReviewService(HttpClient httpClient,
            IConfiguration configuration,
            IWebHostEnvironment env)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.greptile.com/v2/");
            _env = env;

            var apiKey = configuration["Greptile:ApiKey"];
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new InvalidOperationException("Greptile:ApiKey is not configured.");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);

            var githubToken = configuration["Greptile:GithubToken"];
            if (string.IsNullOrWhiteSpace(githubToken))
                throw new InvalidOperationException("Greptile:GithubToken is not configured.");
            _httpClient.DefaultRequestHeaders.Add("X-GitHub-Token", githubToken);
        }

        public async Task<string> ReviewCodeAsync(string repoName)
        {
            var llmInstructions = await LoadAgentInstructionsAsync();
            var loadRepoResult = await LoadRepositoryAsync(repoName);
            var payload = new
            {
                messages = new[]
                {
                    new
                    {
                        id = "1",
                        content = llmInstructions,
                        role = "user"
                    }
                },
                repositories = new[]
                {
                    new
                    {
                        remote = "github",
                        branch = "main",
                        repository = repoName
                    }
                },
                sessionId = 3,
                stream = false,
                genius = true
            };

            for (int attempt = 0; attempt <= 3; attempt++)
            {
                var response = await _httpClient.PostAsJsonAsync(
                    "query", payload);
                if (!response.IsSuccessStatusCode)
                    continue;

                var json = await response.Content.ReadAsStringAsync();

                using (var doc = JsonDocument.Parse(json))
                {
                    var root = doc.RootElement;
                    if (!doc.RootElement.TryGetProperty("message", out var summary))
                        throw new InvalidOperationException("Invalid response from Greptile API.");

                    return summary.GetString();
                }
            }

            throw new InvalidOperationException("Failed to get a successful response from Greptile API after multiple attempts.");
        }

        private async Task<string> LoadRepositoryAsync(string repoName)
        {
            var payload = new
            {
                remote = "github",
                repository = repoName,
                branch = "main",
                reload = true,
                notify = true
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync(
                    "repositories", payload);

                var json = await response.Content.ReadAsStringAsync();
                return json;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<string> LoadAgentInstructionsAsync()
        {
            IFileInfo file = _env.ContentRootFileProvider.GetFileInfo("Resources/llm-instructions.txt");
            if (!file.Exists) throw new FileNotFoundException("Resources/llm-instructions.txt not found.");
            using var stream = file.CreateReadStream();
            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }
    }
}

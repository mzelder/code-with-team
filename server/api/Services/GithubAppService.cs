using api.Services.Interfaces;
using Octokit;
using GraphQL = Octokit.GraphQL;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace api.Services
{
    public class GithubAppService : IGithubAppService
    {
        private readonly IConfiguration _configuration;
        private readonly string _organizationName;

        public GithubAppService(IConfiguration configuration)
        {
            _configuration = configuration;
            _organizationName = configuration["Github:OrganizationName"];
        }

        private string GenerateJwtToken()
        {
            var appId = _configuration["Github:AppId"];
            var privateKeyPath = _configuration["Github:PrivateKeyPath"];
            
            if (string.IsNullOrEmpty(appId))
                throw new InvalidOperationException("Github:AppId is not configured in user secrets");
            
            if (string.IsNullOrEmpty(privateKeyPath))
                throw new InvalidOperationException("Github:PrivateKeyPath is not configured in user secrets");
            
            var fullPath = Path.IsPathRooted(privateKeyPath)
                ? privateKeyPath
                : Path.Combine(Directory.GetCurrentDirectory(), privateKeyPath);
            
            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"Private key file not found at: {fullPath}");
            
            var privateKeyPem = File.ReadAllText(fullPath);

            var rsa = RSA.Create();
            rsa.ImportFromPem(privateKeyPem);

            var securityKey = new RsaSecurityKey(rsa);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256);

            var now = DateTime.UtcNow;
            
            var header = new JwtHeader(credentials);
            var payload = new JwtPayload
            {
                { "iss", appId },
                { "iat", new DateTimeOffset(now.AddSeconds(-60)).ToUnixTimeSeconds() },
                { "exp", new DateTimeOffset(now.AddMinutes(10)).ToUnixTimeSeconds() }
            };

            var jwtToken = new JwtSecurityToken(header, payload);
            var handler = new JwtSecurityTokenHandler();
            
            return handler.WriteToken(jwtToken);
        }

        public async Task<int> GetInstalationAsync(string jwt)
        {
            var appClient = new GitHubClient(new ProductHeaderValue(_organizationName))
            {
                Credentials = new Credentials(jwt, AuthenticationType.Bearer)
            };

            var installation = await appClient.GitHubApps.GetOrganizationInstallationForCurrent(_organizationName);

            return (int)installation.Id;
        }

        public async Task<GitHubClient> GetInstallationAccessClientAsync()
        {
            var jwt = GenerateJwtToken();
            var installationId = await GetInstalationAsync(jwt);

            var appClient = new GitHubClient(new ProductHeaderValue(_organizationName))
            {
                Credentials = new Credentials(jwt, AuthenticationType.Bearer)
            };

            var response = await appClient.GitHubApps.CreateInstallationToken(installationId);
            var authenticatedClient = new GitHubClient(new ProductHeaderValue(_organizationName))
            {
                Credentials = new Credentials(response.Token)
            };

            return authenticatedClient;
        }

        public async Task<GraphQL.Connection> GetGraphQLConnection()
        {
            var restClient = await GetInstallationAccessClientAsync();
            var token = restClient.Credentials.GetToken();

            var authenticatedClient = new GraphQL.Connection(new GraphQL.ProductHeaderValue(_organizationName), token);
            return authenticatedClient;
        }
    }
}

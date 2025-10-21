using api.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GithubBotController : ControllerBase
    {
        private readonly IGithubBotService _githubBotService;
        private readonly IConfiguration _configuration;
        private readonly string _organizationName;

        public GithubBotController(IGithubBotService githubBotService, IConfiguration configuration)
        {
            _githubBotService = githubBotService;
            _configuration = configuration;
            _organizationName = _configuration["Github:OrganizationName"];
        }

        [HttpPost("create-repo")]
        public async Task<IActionResult> CreateRepository([FromQuery] string repoName)
        {
            var repository = await _githubBotService.CreateRepositoryAsync(_organizationName, repoName);
            return Ok(repository);
        }
    }
}

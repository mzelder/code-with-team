using api.Dtos;
using api.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Octokit.Internal;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GithubBotController : BaseAuthorizedController
    {
        private readonly IGithubBotService _githubBotService;
        private readonly IGithubUserService _githubUserService;
        private readonly IConfiguration _configuration;
        private readonly string _organizationName;

        public GithubBotController(IGithubBotService githubBotService, IGithubUserService githubUserService,IConfiguration configuration)
        {
            _githubBotService = githubBotService;
            _githubUserService = githubUserService;
            _configuration = configuration;
            _organizationName = _configuration["Github:OrganizationName"];
        }

        [HttpPost("create-repo")]
        public async Task<IActionResult> CreateRepository([FromQuery] string repoName)
        {
            var repository = await _githubBotService.CreateRepositoryAsync(_organizationName, repoName);
            return Ok(repository);
        }

        [HttpPost("add-collaborator")]
        public async Task<IActionResult> AddCollaborator([FromQuery] string repoName, [FromQuery] string collaboratorUsername)
        {
            try
            {
                await _githubBotService.AddColaboratorAsync(_organizationName, repoName, collaboratorUsername);
                return Ok(new ApiResponseDto(true, "Colaborator have been added successfully"));
            } 
            catch (Exception ex)
            {
                return BadRequest(new ApiResponseDto(false, $"Failed to add collaborator: {ex.Message}"));
            }
        }

        [HttpPost("accept-invitation")]
        public async Task<IActionResult> AcceptInvitation()
        {
            try
            {
                await _githubUserService.AcceptRepositoryInvitationAsync(_organizationName, GetCurrentUserId());
                return Ok(new ApiResponseDto(true, "Repository invitations accepted successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponseDto(false, $"Failed to accept invitations: {ex.Message}"));
            }
        }
    }
}

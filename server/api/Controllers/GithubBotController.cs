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

        public GithubBotController(IGithubBotService githubBotService, 
            IGithubUserService githubUserService, 
            IConfiguration configuration,
            IHostEnvironment env)
        {
            if (!env.IsDevelopment())
            {
                throw new InvalidOperationException
                    ("GithubBotContoller can only be used in Development environment.");
            }
            
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

        [HttpPost("create-repo-from-template")]
        public async Task<IActionResult> CreateRepositoryFromTemplateAsync([FromQuery] string repoName)
        {
            var repository = await _githubBotService.CreateRepositoryFromTemplateAsync(_organizationName, repoName);
            return Ok(repository);
        }

        [HttpPost("add-collaborator-to-repo")]
        public async Task<IActionResult> AddCollaboratorToRepo([FromQuery] string repoName, [FromQuery] string collaboratorUsername)
        {
            try
            {
                await _githubBotService.AddColaboratorToRepoAsync(_organizationName, repoName, collaboratorUsername);
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

        [HttpPost("set-branch-rules")]
        public async Task<IActionResult> SetBranchRules([FromQuery] string repoName)
        {
            try
            {
                await _githubBotService.SetBranchRulesAsync(_organizationName, repoName);
                return Ok(new ApiResponseDto(true, "Branch rules have been set successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponseDto(false, $"Failed to set branch rules: {ex.Message}"));
            }
        }

        [HttpPost("create-project-for-repo")]
        public async Task<IActionResult> CreateProjectForRepository([FromQuery] string repoName, [FromQuery] int repositoryId)
        {
            try
            {
                var repo = await _githubBotService.CreateRepositoryAsync(_organizationName, repoName);
                await _githubBotService.CreateProjectAsync(repo, _organizationName, repoName);
                return Ok(new ApiResponseDto(true, "Project have been created"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponseDto(false, $"Failed to create project: {ex.Message}"));
            }
        }

        [HttpPost("add-collaborator-to-project")]
        public async Task<IActionResult> AddCollaboratorToProject([FromQuery] string repoName)
        {
            try
            {
                // create repo
                var repo = await _githubBotService.CreateRepositoryAsync(_organizationName, repoName);
                
                // create project for the repo
                var project = await _githubBotService.CreateProjectAsync(repo, _organizationName, repoName);

                // add collabs to project
                await _githubBotService.AddColaboratorToProjectAsync(repo, project, _organizationName);

                return Ok(new ApiResponseDto(true, "Project have been created"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponseDto(false, $"Failed to create project: {ex.Message}"));
            }
        }
    }
}

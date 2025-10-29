using api.Dtos.TaskProgress;
using api.Data;
using api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.EntityFrameworkCore;
using api.Dtos;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskProgressController : BaseAuthorizedController
    {
        private readonly ITaskProgressService _taskProgressService;
        private readonly AppDbContext _context;

        public TaskProgressController(ITaskProgressService taskProgressService,
            AppDbContext context)
        {
            _taskProgressService = taskProgressService;
            _context = context;
        }

        [HttpGet("get-tasks")]
        [Authorize]
        public async Task<ActionResult<TaskProgressDto>> GetTasks()
        {
            try
            {
                var userTaskProgress = await _taskProgressService.GetUserTaskProgressAsync(GetCurrentUserId());
                var teamTaskProgress = await _taskProgressService.GetTeamTaskProgressAsync(GetCurrentUserId());
                
                return Ok(new TaskProgressDto
                {
                    CreatedIssues = teamTaskProgress.CreatedIssues,
                    JoinedVideoCall = userTaskProgress.JoinedVideoCall,
                    VisitedRepo = userTaskProgress.VisitedRepo,
                    StartedCoding = userTaskProgress.StartedCoding
                });
            } 
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponseDto(false, ex.Message));
            }
        }
    }
}

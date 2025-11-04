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
        public async Task<ActionResult<IEnumerable<TaskProgressDto>>> GetTasks()
        {
            var tasks = new List<TaskProgressDto>();
            try
            {
                var userTasks = await _taskProgressService.GetUserTaskProgressAsync(GetCurrentUserId());
                var teamTasks = await _taskProgressService.GetTeamTaskProgressAsync(GetCurrentUserId());

                tasks.AddRange(userTasks.Select(ut => new TaskProgressDto(ut)));
                tasks.AddRange(teamTasks.Select(tt => new TaskProgressDto(tt)));

                return tasks;
            } 
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponseDto(false, ex.Message));
            }
        }
    }
}

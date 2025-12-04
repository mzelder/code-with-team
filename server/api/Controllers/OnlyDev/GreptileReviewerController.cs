using api.Dtos;
using api.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers.OnlyDev
{
    [Route("api/[controller]")]
    [ApiController]
    public class GreptileReviewerController : ControllerBase
    {
        private readonly IGreptileReviewService _codeReviewService;

        public GreptileReviewerController(IGreptileReviewService codeReviewService,
            IHostEnvironment env)
        {
            if (!env.IsDevelopment())
            {
                throw new InvalidOperationException
                    ("GithubBotContoller can only be used in Development environment.");
            }
    
            _codeReviewService = codeReviewService;
        }

        [HttpPost("review-code")]
        public async Task<ActionResult> ReviewCode([FromQuery] string repoName)
        {
            try
            {
                var result = await _codeReviewService.ReviewCodeAsync(repoName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponseDto(false, ex.Message));
            }
        }
    }
}

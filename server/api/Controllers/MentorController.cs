using api.Dtos.TaskProgress;
using api.Data;
using api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.EntityFrameworkCore;
using api.Dtos.Mentor;
using api.Models.Mentor;
using api.Dtos;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MentorController : BaseAuthorizedController
    {
        private readonly AppDbContext _context;
        private readonly IMentorService _mentorService;

        public MentorController(AppDbContext context, IMentorService mentorService)
        {
            _context = context;
            _mentorService = mentorService;
        }

        [HttpGet("get-mentor")]
        public async Task<ActionResult<MentorDto>> GetMentorStatus()
        {
            try
            {
                return await _mentorService.GetMentorStatusAsync(GetCurrentUserId());
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponseDto(false, ex.Message));
            }

        }

        [HttpGet("get-questions")]
        public async Task<ActionResult<MentorQuestionDto>> GetMentorQuestions()
        {
            try
            {
                return await _mentorService.GetMentorQuestionsAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponseDto(false, ex.Message));
            }
        }

        [HttpPost("submit-form")]
        public async Task<ActionResult> SubmitMentorForm([FromBody] MentorFormDto mentorFormDto)
        {
            try
            {
                await _mentorService.SubmitMentorFormAsync(GetCurrentUserId(), mentorFormDto);
                return Ok(new ApiResponseDto(true, "Mentor form submitted successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponseDto(false, ex.Message));

            }
        }

        [HttpPost("accept-application/{mentorFormId}")] //change on prod
        public async Task<ActionResult> AcceptMentorApplication(int mentorFormId)
        {
            try
            {
                await _mentorService.AcceptMentorApplicationAsync(mentorFormId);
                return Ok(new ApiResponseDto(true, "Mentor application accepted successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponseDto(false, ex.Message));
            }
        }
    }
}
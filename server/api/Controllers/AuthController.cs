using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Models;
using System.Security.Claims;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using api.Dtos.Auth;
using api.Dtos;
using api.Services.Interfaces;
using api.Extensions;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IAuthService _authService;

        public AuthController(AppDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponseDto>> Register([FromBody] RegisterDto dto)
        {
            var authorizedUser = await _authService.RegisterAsync(dto);
            if (authorizedUser == null) 
            {
               return BadRequest(new ApiResponseDto(false, "Invalid username or password. Please try again."));
            }

            var identity = authorizedUser.ToClaimsIdentity();
            await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(identity));

            return Ok(new ApiResponseDto(true, "Login successful"));
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponseDto>> Login([FromBody] LoginDto dto)
        {
            var authorizedUser = await _authService.LoginAsync(dto);
            if (authorizedUser == null)
            {
                return BadRequest(new ApiResponseDto(false, "Invalid username or password. Please try again."));
            }

            var identity = authorizedUser.ToClaimsIdentity();
            await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(identity));

            return Ok(new ApiResponseDto(true, "Login successful"));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Ok(new ApiResponseDto(true, "Logged out"));
        }

        [HttpGet("check")]
        [Authorize]
        public IActionResult CheckAuthentication()
        {
            return Ok(new {
                IsAuthenticated = true,
                User = User.Identity?.Name
            });
        }

        [HttpPost("github/callback")]
        public async Task<ActionResult<ApiResponseDto>> GithubCallback([FromBody] GithubCallbackDto dto) 
        { 
            var authorizedUser = await _authService.GithubCallbackAsync(dto);
            if (authorizedUser == null) 
            {
               return BadRequest(new ApiResponseDto(false, "GitHub authentication failed. Please try again."));
            }

            var identity = authorizedUser.ToClaimsIdentity();
            await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(identity));
            
            return Ok(new ApiResponseDto(true, "Login successful via GitHub"));
        }
    }
}

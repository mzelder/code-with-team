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

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponseDto>> Register([FromBody] RegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
            {
                return BadRequest(new ApiResponseDto(false, "Username already exists."));
            }

            if (dto.Password != dto.ConfirmPassword)
            {
                return BadRequest(new ApiResponseDto(false, "Passwords do not match."));
            }

            var user = new User
            {
                Username = dto.Username,
                Password = dto.Password
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponseDto(true, "User registered."));
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponseDto>> Login([FromBody] LoginDto dto)
        {
            var existingUser = await _context.Users.
                FirstOrDefaultAsync(u => u.Username == dto.Username);

            if (existingUser == null || existingUser.Password != dto.Password)
            {
                return Unauthorized(new ApiResponseDto(false, "Invalid username or password"));
            }

            var claims = new List<Claim> { 
                new Claim(ClaimTypes.Name, existingUser.Username),
                new Claim(ClaimTypes.NameIdentifier, existingUser.Id.ToString())
            };
            var identity = new ClaimsIdentity(claims, "Cookies");
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
        public IActionResult CheckAuth()
        {
            return Ok(new {
                IsAuthenticated = true,
                User = User.Identity?.Name
            });
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Models;
using System.Security.Claims;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using api.Dtos.Auth;

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
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
            {
                return BadRequest(new { message = "Username already exists." });
            }

            if (dto.Password != dto.ConfirmPassword)
            {
                return BadRequest(new { message = "Passwords do not match."});
            }

            var user = new User
            {
                Username = dto.Username,
                Password = dto.Password
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message ="User registered.", success = true});
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var existingUser = await _context.Users.
                FirstOrDefaultAsync(u => u.Username == dto.Username);

            if (existingUser == null)
            {
                return Unauthorized(new { message = "Invalid username or password"});
            }

            if (existingUser.Password != dto.Password)
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            var claims = new List<Claim> { 
                new Claim(ClaimTypes.Name, existingUser.Username),
                new Claim(ClaimTypes.NameIdentifier, existingUser.Id.ToString())
            };
            var identity = new ClaimsIdentity(claims, "Cookies");
            await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(identity));

            return Ok(new { message = "Login successful", success = true });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Ok(new { message = "Logged out", success = true });
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

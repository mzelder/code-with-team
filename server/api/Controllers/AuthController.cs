using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Models;
using api.Dtos;
using System.Security.Claims;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

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
                return BadRequest("Username already exists");
            }

            if (dto.Password != dto.ConfirmPassword)
            {
                return BadRequest("Password do not match");
            }

            var user = new User
            {
                Username = dto.Username,
                Password = dto.Password
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registred");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var existingUser = await _context.Users.
                FirstOrDefaultAsync(u => u.Username == dto.Username);

            if (existingUser == null)
            {
                return Unauthorized("Invalid username or password");
            }

            if (existingUser.Password != dto.Password)
            {
                return Unauthorized("Invalid username or password");
            }

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, existingUser.Username) };
            var identity = new ClaimsIdentity(claims, "Cookies");
            await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(identity));

            return Ok("Login successful");
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Ok("Logged out");
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            return Ok(new { User = User.Identity?.Name });
        }
    }
}

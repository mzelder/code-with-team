using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Models;
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
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            // Compare passwords
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
            {
                return BadRequest("Username already exists");
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

            return Ok("Login successful");
        }
    }

}

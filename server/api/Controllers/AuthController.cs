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
using Octokit;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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

            var user = new Models.User
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

        [HttpPost("github/callback")]
        public async Task<ActionResult<ApiResponseDto>> GithubCallback([FromBody] GithubCallbackDto dto) 
        { 
            var clientId = _configuration["GitHub:ClientId"];
            var clientSecret = _configuration["GitHub:ClientSecret"];

            var github = new GitHubClient(new ProductHeaderValue("CodeWithTeam"));

            var tokenRequest = new OauthTokenRequest(clientId, clientSecret, dto.Code);
            var token = await github.Oauth.CreateAccessToken(tokenRequest);

            var authenticatedClient = new GitHubClient(new ProductHeaderValue("CodeWithTeam"))
            {
                Credentials = new Credentials(token.AccessToken)
            };

            var githubUser = await authenticatedClient.User.Current();
            
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username== githubUser.Login);

            if (existingUser == null)
            {
                existingUser = new Models.User
                {
                    Username = githubUser.Login,
                    Password = Guid.NewGuid().ToString()
                };
                _context.Users.Add(existingUser);
                await _context.SaveChangesAsync();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, existingUser.Username),
                new Claim(ClaimTypes.NameIdentifier, existingUser.Id.ToString())
            };
            var identity = new ClaimsIdentity(claims, "Cookies");
            await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(identity));

            return Ok(new ApiResponseDto(true, "GitHub login successful"));
        }
    }
}

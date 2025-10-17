using api.Data;
using api.Dtos;
using api.Dtos.Auth;
using api.Dtos.Matchmaking;
using api.Models;
using api.Services.Interfaces;
using Azure.Identity;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Octokit;
using System.Security.Claims;


namespace api.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthorizedUser> RegisterAsync(RegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
            {
                return null;
            }

            if (dto.Password != dto.ConfirmPassword)
            {
                return null;
            }

            var user = new Models.User
            {
                Username = dto.Username,
                Password = dto.Password
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new AuthorizedUser
            {
                Id = user.Id,
                Username = user.Username
            };
        }

        public async Task<AuthorizedUser> LoginAsync(LoginDto dto)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == dto.Username);

            if (existingUser == null || existingUser.Password != dto.Password)
            {
                return null;
            }

            return new AuthorizedUser
            {
                Id = existingUser.Id,
                Username = existingUser.Username
            };
        }
        
        public async Task<AuthorizedUser> GithubCallbackAsync(GithubCallbackDto dto)
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
                .FirstOrDefaultAsync(u => u.Username == githubUser.Login);

            if (existingUser == null)
            {
                existingUser = new Models.User
                {
                    Username = githubUser.Login,
                    Password = Guid.NewGuid().ToString()
                };
            }
            _context.Users.Add(existingUser);
            await _context.SaveChangesAsync();

            return new AuthorizedUser
            {
                Id = existingUser.Id,
                Username = existingUser.Username
            };
        } 
    }
}
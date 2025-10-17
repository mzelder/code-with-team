using api.Dtos;
using api.Dtos.Auth;
using api.Models;
namespace api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthorizedUser> RegisterAsync(RegisterDto dto); 
        Task<AuthorizedUser> LoginAsync(LoginDto dto);
        Task<AuthorizedUser> GithubCallbackAsync(GithubCallbackDto dto);
    }
}

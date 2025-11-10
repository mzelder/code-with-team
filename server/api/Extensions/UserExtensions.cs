using api.Dtos.Auth;
using api.Models;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.Claims;

namespace api.Extensions
{
    public static class UserExtensions
    {
        public static List<Claim> ToClaims(this AuthorizedUser user)
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };
        }

        public static ClaimsIdentity ToClaimsIdentity(this AuthorizedUser user, string authenticationType = "Cookies")
        {
            return new ClaimsIdentity(user.ToClaims(), authenticationType);
        }

        public static int GetCurrentUserId(this ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("User ID not found in claims");
            }
            return int.Parse(userIdClaim.Value);
        }
    }
}
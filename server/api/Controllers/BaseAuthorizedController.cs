using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace api.Controllers
{
    [Authorize]
    [ApiController]
    public abstract class BaseAuthorizedController : ControllerBase
    {
        protected int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("User ID not found in claims");
            }

            return int.Parse(userIdClaim.Value);
        }
    }
}
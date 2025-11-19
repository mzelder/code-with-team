using api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Authorize]
    [ApiController]
    public abstract class BaseAuthorizedController : ControllerBase
    {
        protected int GetCurrentUserId() => User.GetCurrentUserId();
    }
}
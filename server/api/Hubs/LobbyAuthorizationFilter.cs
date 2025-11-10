using api.Data;
using api.Extensions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace api.Hubs
{
    public class LobbyAuthorizationFilter : IHubFilter
    {
        private readonly AppDbContext _context;

        public LobbyAuthorizationFilter(AppDbContext context)
        {
            _context = context;
        }

        public async ValueTask<object?> InvokeMethodAsync(
            HubInvocationContext invocationContext,
            Func<HubInvocationContext, ValueTask<object?>> next)
        {
            var methodParams = invocationContext.HubMethod.GetParameters();
            var lobbyIdParamIndex = Array.FindIndex(methodParams, p => p.Name == "lobbyId");

            if (lobbyIdParamIndex >= 0 &&
                invocationContext.HubMethodArguments[lobbyIdParamIndex] is string lobbyId)
            {
                if (!int.TryParse(lobbyId, out int lobbyIdInt))
                    throw new HubException("Invalid lobby ID");

                var userId = invocationContext.Context.User!.GetCurrentUserId();

                var isInLobby = await _context.LobbyMembers
                    .AnyAsync(lm => lm.UserId == userId && lm.LobbyId == lobbyIdInt);

                if (!isInLobby)
                    throw new HubException("Unauthorized: You are not a member of this lobby");
            }

            return await next(invocationContext);
        }
    }

}

using api.Data;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace api.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly AppDbContext _context;

        public ChatHub(AppDbContext context)
        {
            _context = context;
        }

        public async Task SendMessage(string lobbyId, string userName, string message)
        {
            // ADD: deletaing messages from lobby after deleating lobby
            
            var timestamp = DateTime.UtcNow.ToString("HH:mm");
            var user = _context.Users.FirstOrDefault(u => u.Username == userName);

            if (user == null)
            {
                throw new Exception("User is not existing");
            }

            if (!int.TryParse(lobbyId, out int lobbyIdInt))
            {
                throw new Exception("Invalid lobby ID");
            }

            var chatMessage = new ChatMessage
            {
                LobbyId = lobbyIdInt,
                UserId = user.Id,
                Message = message,
                CreatedAt = DateTime.UtcNow
            };

            _context.ChatMessages.Add(chatMessage);
            await _context.SaveChangesAsync();

            await Clients.Group(lobbyId).SendAsync("ReceiveMessage", userName, message, timestamp);
        }

        public async Task JoinLobby(string lobbyId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, lobbyId);
        } 

        public async Task LeaveLobby(string lobbyId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, lobbyId);
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}

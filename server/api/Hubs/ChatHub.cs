using api.Data;
using api.Dtos.Chat;
using api.Models;
using api.Extensions;
using api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace api.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;

        public ChatHub(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task SendMessage(ChatMessageDto messageDto, string lobbyId)
        {
            var savedMessage = await _chatService.SaveMessageAsync(messageDto);
            await Clients.Group(lobbyId).SendAsync("ReceiveMessage", savedMessage); 
        }

        public async Task SendMeetingProposal(MeetingProposalDto proposalDto, string lobbyId)
        {
            try
            {
                var savedProposal = await _chatService.SaveMeetingProposalAsync(proposalDto);
                await Clients.Group(lobbyId).SendAsync("ReceiveMeetingProposal", savedProposal);
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("Error", ex.Message);
                return;
            }
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

using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using OpenLibraryServer.Models;
using OpenLibraryServer.Service.HelperFunctions;
using OpenLibraryServer.Service.Interfaces;

namespace OpenLibraryServer.Service
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;

        public ChatHub(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task JoinChat(Guid chatId)
        {
            var messages = _chatService.GetMessagesByChat(chatId);
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
            await Clients.Caller.SendAsync("ReceiveMessageHistory", await messages);
        }
        
        public Task LeaveChat(Guid chatId)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());
        }
        
        public async Task SendMessage(SendMessageRequest request)
        {
            var userId = TokenHelpers.GetUserId(Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value); 
            var messageTO = _chatService.AddMessage(request.Message, userId, request.ChatId);
            await Clients.Group(request.ChatId.ToString()).SendAsync("ReceiveMessage", await messageTO);
        }
        
        public class SendMessageRequest
        {
            public Guid ChatId { get; set; }
            public string Message { get; set; }
        }
    }
}
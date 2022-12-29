using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenLibraryServer.Models;
using OpenLibraryServer.Models.DTOs;

namespace OpenLibraryServer.Service.Interfaces
{
    public interface IChatService
    {
        Task<Chat> CreateChat(ChatTO chatTo);
        Task<MessageTO> AddMessage(string message, Guid userId, Guid chatId);
        Task<ICollection<MessageTO>> GetMessagesByChat(Guid chatId);
        Task<IEnumerable<ChatTO>> GetChatsByUser(Guid userId);
        Task AssignUserToChat(Guid userId, Guid chatId);
        Task UnassignUserFromChat(Guid userId, Guid chatId);
        Task<ChatTO> GetById(Guid chatId);
    }
}
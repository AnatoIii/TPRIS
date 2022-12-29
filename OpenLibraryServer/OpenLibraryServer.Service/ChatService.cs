using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OpenLibraryServer.DataAccess;
using OpenLibraryServer.Models;
using OpenLibraryServer.Models.DTOs;
using OpenLibraryServer.Service.HelperFunctions;
using OpenLibraryServer.Service.Interfaces;

namespace OpenLibraryServer.Service
{
    public class ChatService : IChatService
    {
        private readonly OpenLibraryServerDBContext _context;

        public ChatService(OpenLibraryServerDBContext context)
        {
            _context = context;
        }

        public async Task<Chat> CreateChat(ChatTO chatTo)
        {
            var chat = await _context.Chats.AddAsync(new Chat()
            {
                Name = chatTo.Name
            });
            await _context.SaveChangesAsync();
            return chat.Entity;
        }

        public async Task<MessageTO> AddMessage(string message, Guid userId, Guid chatId)
        {
            var chat = await _context.Chats.FirstOrDefaultAsync(c => c.ChatId == chatId);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            EntityHelpers.CheckEntityExists(chat, $"Chat with id {chatId} doesn't exist");
            EntityHelpers.CheckEntityExists(user, $"User with id {userId} doesn't exist");
            var newMessage = await _context.ChatMessages.AddAsync(new ChatMessage()
            {
                Chat = chat,
                User = user,
                Message = message,
                ChatId = chatId,
                UserId = userId,
                DateTime = DateTime.Now
            });
            await _context.SaveChangesAsync();
            var entity = newMessage.Entity;
            chat.ChatMessage = entity;
            _context.Update(chat);
            await _context.SaveChangesAsync();
            return ConvertToTO(entity, user.UserName);
        }

        private static MessageTO ConvertToTO(ChatMessage message, string userName)
        {
            return new MessageTO()
            {
                Message = message.Message,
                Username = userName,
                DateTime = message.DateTime,
                UserId = message.UserId ?? Guid.Empty,
                ChatId = message.ChatId
            };            
        }

        public async Task<ICollection<MessageTO>> GetMessagesByChat(Guid chatId)
        {
            var dbMessages = await _context.ChatMessages.Where(cm => cm.ChatId == chatId)
                .Include(cm => cm.Chat).Include(cm => cm.User)
                .OrderBy(cm => cm.DateTime).Take(100).ToListAsync();
            return dbMessages.Select(m => ConvertToTO(m, m.User.UserName)).ToList();
        }

        public async Task<IEnumerable<ChatTO>> GetChatsByUser(Guid userId)
        {
            var chats = await _context.UserChats.Where(uc => uc.UserId == userId)
                .Include(uc => uc.Chat!)
                    .ThenInclude(c=> c.ChatMessage)
                        .ThenInclude(cm => cm.User)
                .Select(uc => uc.Chat).ToListAsync();
            var chatIds = chats.Select(c => c.ChatId);
            var books = await _context.Books.Where(b => chatIds.Contains(b.ChatId))
                .ToDictionaryAsync(b => b.ChatId);
            return ConvertChatsToTOs(chats, books);
        }

        private IEnumerable<ChatTO> ConvertChatsToTOs(IEnumerable<Chat> chat, Dictionary<Guid,Book> booksDictionary)
        {
            return chat.Select(c => new ChatTO()
            {
                ChatId = c.ChatId,
                Name = c.Name,
                LastMessage = ConvertMessageToTO(c.ChatMessage),
                ImageURL = booksDictionary[c.ChatId].PictureURL
            }).ToList();
        }

        public async Task AssignUserToChat(Guid userId, Guid chatId)
        {
            var userChat = await _context.UserChats.Where(uc => uc.ChatId == chatId && uc.UserId == userId)
                .FirstOrDefaultAsync();
            if (userChat != null)
            {
                return;
            }
            var newUserChat = new UserChats()
            {
                UserId = userId,
                ChatId = chatId
            };
            await _context.UserChats.AddAsync(newUserChat);
            await _context.SaveChangesAsync();
        }

        public async Task UnassignUserFromChat(Guid userId, Guid chatId)
        {
            var entity = await _context.UserChats.FirstOrDefaultAsync(uc => uc.ChatId == chatId && uc.UserId == userId);
            EntityHelpers.CheckEntityExists(entity, $"No userChat for userId: {userId} and chatId: {chatId}");
            _context.UserChats.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<ChatTO> GetById(Guid chatId)
        {
            var book = await _context.Books.
                Where(b => b.ChatId == chatId)
                .Include(b => b.Chat)
                    .ThenInclude(c => c.ChatMessage)
                        .ThenInclude(cm => cm.User)
                .FirstOrDefaultAsync(c => c.ChatId == chatId);
            EntityHelpers.CheckEntityExists(book, $"No book for chatId: {chatId}");
            return new ChatTO()
            {
                Name = book.Chat.Name,
                ChatId = chatId,
                LastMessage = ConvertMessageToTO(book.Chat.ChatMessage),
                ImageURL = book.PictureURL
            };

        }

        private MessageTO ConvertMessageToTO(ChatMessage message)
        {
            return message != null ? new MessageTO()
            {
                Message = message.Message,
                Username = message.User.UserName,
                DateTime = message.DateTime,
                UserId = message.UserId ?? Guid.Empty,
                ChatId = message.ChatId
            } : null;
        }
    }
}
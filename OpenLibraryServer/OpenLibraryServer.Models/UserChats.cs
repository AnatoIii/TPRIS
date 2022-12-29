using System;
using System.ComponentModel.DataAnnotations;
using OpenLibraryServer.Models.DTOs;

namespace OpenLibraryServer.Models
{
    public class UserChats
    {
        [Key]
        public Guid UserChatId { get; set; }
        public Guid UserId { get; set; }
        public Guid? ChatId { get; set; }
        
        public User User { get; set; }
        public Chat? Chat { get; set; }
    }
}
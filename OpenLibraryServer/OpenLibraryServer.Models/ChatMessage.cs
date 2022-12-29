using System;
using System.ComponentModel.DataAnnotations;

namespace OpenLibraryServer.Models
{
    public class ChatMessage
    {
        [Key]
        public Guid ChatMessageId { get; set; }
        public Guid ChatId { get; set; }
        public Guid? UserId { get; set; }
        public DateTime DateTime { get; set; }
        public string Message { get; set; }
        
        public Chat Chat { get; set; }
        public User? User { get; set; }
    }
}
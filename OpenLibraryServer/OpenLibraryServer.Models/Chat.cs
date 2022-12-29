using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace OpenLibraryServer.Models
{
    public class Chat
    {
        [Key]
        public Guid ChatId { get; set; }
        public string Name { get; set; }
        public Guid? ChatMessageId { get; set; }

        public ChatMessage ChatMessage { get; set; }
    }
}
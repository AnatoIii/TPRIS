using System;
using System.ComponentModel.DataAnnotations;

namespace OpenLibraryServer.Models
{
    public class Token
    {
        [Key]
        public Guid Id { get; set; }
        public string RefreshToken { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public User User { get; set; }
    }
}
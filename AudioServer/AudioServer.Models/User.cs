using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AudioServer.Models
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public ICollection<Token> Tokens { get; set; }
    }
}
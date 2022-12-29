using System;

namespace AudioServer.Models.DTOs
{
    public class UserTO
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
    }
}
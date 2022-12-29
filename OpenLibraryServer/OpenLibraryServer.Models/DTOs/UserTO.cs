using System;

namespace OpenLibraryServer.Models.DTOs
{
    public class UserTO
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string UITheme { get; set; }
    }
}
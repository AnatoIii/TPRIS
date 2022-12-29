using System;

namespace AudioServer.Models.DTOs
{
    public class SaveFileTO
    {
        public string FileContent { get; set; }
        public string RelativePath { get; set; }
    }
}

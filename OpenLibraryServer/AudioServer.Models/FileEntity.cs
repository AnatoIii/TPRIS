using System;
using System.ComponentModel.DataAnnotations;

namespace AudioServer.Models
{
    public class FileEntity
    {
        [Key]
        public int FileId { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string FileURL { get; set; }
        public Guid CreatorId { get; set; }
    }
}
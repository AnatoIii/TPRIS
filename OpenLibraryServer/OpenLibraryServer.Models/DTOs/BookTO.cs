using System;
using System.Collections;
using System.Collections.Generic;

namespace OpenLibraryServer.Models.DTOs
{
    public class BookTO
    {
        public Guid BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string ImageLink { get; set; }
        public string FileURL { get; set; }
        public string Status { get; set; }
        public Guid ChatId { get; set; }
        
        public ICollection<Genre> Genres { get; set; }
    }
}
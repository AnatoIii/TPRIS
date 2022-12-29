using System.Collections.Generic;

namespace OpenLibraryServer.Models.DTOs
{
    public class CreateBookTO
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string ImageLink { get; set; }
        public string FileURL { get; set; }
        
        public ICollection<int> Genres { get; set; }
    }
}
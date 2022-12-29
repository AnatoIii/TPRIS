using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OpenLibraryServer.Models
{
    public class Book
    {
        [Key]
        public Guid BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string PictureURL { get; set; }
        public string FileURL { get; set; }
        public string Status { get; set; }
        public Guid ChatId { get; set; }
        
        public Chat Chat { get; set; }
        public ICollection<BookGenres> BookGenres { get; set; }
    }
}
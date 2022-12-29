using System;
using System.ComponentModel.DataAnnotations;

namespace OpenLibraryServer.Models
{
    public class BookGenres
    {
        [Key]
        public Guid BookGenreId { get; set; }
        public Guid BookId { get; set; }
        public int GenreId { get; set; }
        
        public Book Book { get; set; }
        public Genre Genre { get; set; }
    }
}
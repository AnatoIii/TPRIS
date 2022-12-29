using System.Collections;
using System.Collections.Generic;

namespace OpenLibraryServer.Models.DTOs
{
    public class FilterResponse
    {
        public IEnumerable<BookTO> BookTOs { get; set; }
        public int Count { get; set; }
    }
}
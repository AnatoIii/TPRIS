namespace OpenLibraryServer.Models.DTOs
{
    public class BookFilter
    {
        public int Count { get; set; } = 0;
        public int Offset { get; set; } = 0;
        public string Title { get; set; } = "";
        public string Author { get; set; } = "";
        public int[] Genres { get; set; } = new int[] { };
    }
}
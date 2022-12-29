namespace AudioServer.Models.DTOs
{
    public class CreateEditFileTO
    {
        public int FileId { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string Base64File { get; set; }
    }
}
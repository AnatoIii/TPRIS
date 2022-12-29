namespace AudioServer.FileServer.DTOs
{
    public class SaveFileRequest
    {
        public string FileContent { get; set; }
        public string RelativePath { get; set; }
    }
}
